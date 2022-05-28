using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        //** NEW
        public GameObject normalBody;
        public GameObject wormholeBody;
        private string parentWormholeName;
        public bool stopMotion = false;
        public Vector2 lastDeathPosCenter; // used to hold position of player while dying
        public Vector2 lastDeathPosBottom;
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
            modifiers.Add("normal");
            modifierColors.Add("normal", "#ffffff");
            activeModifier = modifiers[0];

            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            Color playerColor;
            ColorUtility.TryParseHtmlString(modifierColors["normal"], out playerColor);
            spriteRenderer.color = playerColor;
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
                    int curModifier = modifiers.BinarySearch(activeModifier);

                    if (curModifier < numModifiers)
                    {
                        activeModifier = modifiers[curModifier + 1];
                    } else {
                        activeModifier = modifiers[0];
                    }

                    Color playerColor;
                    ColorUtility.TryParseHtmlString(modifierColors[activeModifier], out playerColor);
                    spriteRenderer.color = playerColor;

                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        public void spawnBody()
        {
            float newScale = 1.0f;
            GameObject bodyType = normalBody;
            Vector2 spawnPos = lastDeathPosBottom;
            // Spawn the body on death
            switch (this.activeModifier)
            {
                case ("normal"):
                    newScale *= 1;
                    break;
                case ("big"):
                    newScale *= 2;
                    break;
                case ("wormhole"):
                    bodyType = wormholeBody;
                    spawnPos = lastDeathPosCenter;
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

            newBody.transform.parent = GameObject.Find("DeadBodies").transform;


            // Reset sprite size to normal
            bodyType.transform.localScale /= newScale;
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

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
        public enum Modifiers
        {
            Normal,
            Big
        }
    }
}