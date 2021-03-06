using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    public class ModifierController : MonoBehaviour
    {
        internal Collider2D _collider;
        internal AudioSource _audio; 
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        public string modifierType;
        public string modifierColor;

        public bool isLinked = false; // only used for wormhole modifier
        private GameObject wormholeExit;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.modifiers.Add(modifierType);
                player.modifierColors.Add(modifierType, modifierColor);
                string sceneName = gameObject.scene.name + "Objects";
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                this.gameObject.transform.parent = GameObject.Find(sceneName).transform.Find("GrabbedPowerups").transform;
            }
        }
        public void setWormholeExit(GameObject exit)
        {
            this.wormholeExit = exit;
        }
        public GameObject getWormholeExit()
        {
            return this.wormholeExit;
        }
    }
}