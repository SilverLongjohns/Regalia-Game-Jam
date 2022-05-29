using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        //** NEW
        public GameObject stoneBody;
        public GameObject iceBody;
        public GameObject wormholeBody;
        private string parentWormholeName;
        public bool stopMotion = false;
        public Vector2 lastDeathPosCenter; // used to hold position of player while dying
        public Vector2 lastDeathPosBottom;
        private bool isKicking = false;
        private int kickFrameCount = 0;
        //** END NEW

        public List<string> modifiers = new List<string>();
        public string activeModifier;
        public Dictionary<string, string> modifierColors = new Dictionary<string, string>();

        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            //modifiers.Add("normal");
            //modifierColors.Add("normal", "#ffffff");
            //activeModifier = modifiers[0];
            activeModifier = "normal";

            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            //Color playerColor;
            //ColorUtility.TryParseHtmlString(modifierColors["normal"], out playerColor);
            //spriteRenderer.color = playerColor;
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    int numModifiers = modifiers.Count;
                    int newModifier = modifiers.IndexOf(activeModifier) + 1;

                    if (newModifier < numModifiers)
                    {
                        activeModifier = modifiers[newModifier];
                    } else {
                        
                        activeModifier = modifiers[0];
                    }

                    Color playerColor;
                    ColorUtility.TryParseHtmlString(modifierColors[activeModifier], out playerColor);
                    spriteRenderer.color = playerColor;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    // Reset last body
                    string deadBodyContainer = gameObject.scene.name + "Bodies";
                    int bodyToDestroy = GameObject.Find(deadBodyContainer).transform.childCount - 1;
                    Destroy(GameObject.Find(deadBodyContainer).transform.GetChild(bodyToDestroy).gameObject);

                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    // Kick Ice Body
                    Schedule<PlayerKicks>();
                } 
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            UpdateKickHitBox();
            base.Update();
        }

        public void spawnBody()
        {
            float newScale = 1.0f;
            lastDeathPosCenter = gameObject.GetComponent<Rigidbody2D>().position;
            lastDeathPosBottom = transform.GetChild(0).position;

            GameObject bodyType = stoneBody;
            Vector2 spawnPos = lastDeathPosBottom;
            // Spawn the body on death
            switch (this.activeModifier)
            {
                case ("stone"):
                    bodyType = stoneBody;
                    break;
                case ("ice"):
                    bodyType = iceBody;
                    break;
                case ("wormhole"):
                    bodyType = wormholeBody;
                    spawnPos = lastDeathPosCenter;
                    break;
                case ("normal"):
                    Debug.Log("how the hell are you here?");
                    break;
            }

            // Adjust sprite size to the new scale
            // Note: Collider gets scaled at the same time
            bodyType.transform.localScale *= newScale;

            // Spawn body
            GameObject newBody = Instantiate(bodyType, spawnPos, Quaternion.identity);
            if (this.activeModifier == "wormhole")
            {
                GameObject wormholeParent = GameObject.Find(this.getWormholeParent());
                wormholeParent.GetComponent<ModifierController>().setWormholeExit(newBody);
                wormholeParent.GetComponent<ModifierController>().isLinked = true;
            }

            // move new body to the persistent container
            string deadBodyContainer = gameObject.scene.name + "Bodies";
            newBody.transform.parent = GameObject.Find(deadBodyContainer).transform;

            // Reset sprite size to normal
            bodyType.transform.localScale /= newScale;
        }

        private void UpdateKickHitBox()
        {
            // when kick is enabled, count for 5 frames then disable
            if (isKicking)
            {
                if (kickFrameCount > 1)
                {
                    kickFrameCount += 1;
                }
                else
                { 
                    gameObject.transform.Find("Kick").gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            
            Vector3 newDirection;
            if (velocity.x > 0)
            {
                newDirection = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else if (velocity.x < 0)
            {
                newDirection = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                return;
            }
            gameObject.transform.Find("Kick").localScale = newDirection;
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            targetVelocity = move * maxSpeed;
            if (stopMotion)
            {
                velocity.y = 0;
                this.GetComponent<Rigidbody2D>().position = lastDeathPosCenter;
            }
        }

        public void setWormholeParent(string parentName)
        {
            this.parentWormholeName = parentName;
        }

        public string getWormholeParent()
        {
            return this.parentWormholeName;
        }

        public bool getKicking()
        {
            return isKicking;
        }

        public void toggleKick()
        {
            // toggles between on/off
            gameObject.transform.Find("Kick").gameObject.GetComponent<BoxCollider2D>().enabled ^= true;
            isKicking ^= true;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}