using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{

    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyController enemy;
        public PlayerController player;
        public Animator anim;

        public int playerBounce;

        public bool hungryEnemy = true;


        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var willHurtPlayer = (hungryEnemy ? true : false);

            anim = enemy.GetComponent<Animator>();

            if (willHurtPlayer)
            {
                Debug.Log("Hurt player");

                Schedule<PlayerDeath>();

                hungryEnemy = false;

                playerBounce = 8;

                
                if(enemy.path)
                {
                    enemy.path.startPosition = new Vector3(0f, 0f, 0f);
                    enemy.path.endPosition = new Vector3(0f, 0f, 0f);
                }
               
                anim.SetTrigger("killedPlayer");
            }
            else
            {
                player.Bounce(playerBounce);
            }
        }
    }
}