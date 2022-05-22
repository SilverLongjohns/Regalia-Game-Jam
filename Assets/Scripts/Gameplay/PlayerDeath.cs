﻿using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        //** NEW

        

        //** End NEW

        public override void Execute()
        {
            var player = model.player;
            if (player.health.IsAlive)
            {
                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                // Spawn the body on death
                Vector2 dims = player.GetComponent<BoxCollider2D>().size;
                float x = 0;
                float y = 0;
                switch (player.modifier)
                {
                    case ("normal"):
                        x = 0.2f;
                        y = 0.2f;
                        break;
                    case ("big"):
                        x = 3.0f;
                        y = 3.0f;
                        break;
                }
                Vector2 newScaleVect = new Vector2(x, y);
                player.spawnBody(dims, newScaleVect);
                Debug.Log(player.modifier);


                if (player.audioSource && player.ouchAudio)
                    player.audioSource.PlayOneShot(player.ouchAudio);
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(2);
            }
        }
    }
}