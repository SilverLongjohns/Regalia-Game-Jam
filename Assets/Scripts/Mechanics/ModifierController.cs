﻿using System.Collections;
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
                var ev = Schedule<PlayerModifierCollision>();
                ev.player = player;
                ev.modifier = this;
            }
        }
/*
        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
        }
*/
    }
}