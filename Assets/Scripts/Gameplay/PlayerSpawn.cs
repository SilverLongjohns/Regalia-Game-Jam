using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public bool dropBody;

        public override void Execute()
        {
            var player = model.player;
            player.gameObject.GetComponent<SpriteRenderer>().enabled = true; //re-enable in the case of getting eaten
            player.collider2d.enabled = false; // Gets turned back on in PlayerSpawn.cs
            player.controlEnabled = false;
            if (player.audioSource && player.respawnAudio)
                player.audioSource.PlayOneShot(player.respawnAudio);
            player.health.Increment();

            if (dropBody)
            {
                player.spawnBody();
            }
            
            //player.animator.SetBool("dead", false);
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.Grounded;
            model.virtualCamera.m_Follow = player.transform;
            model.virtualCamera.m_LookAt = player.transform;
            Simulation.Schedule<EnablePlayerInput>(2f);

            //** NEW
            player.collider2d.enabled = true;
            player.stopMotion = false;
            //** END NEW
        }
    }
}