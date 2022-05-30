using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;
using System.Collections;
using System.Collections.Generic;

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
        public float bounceHeight;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            anim = enemy.GetComponent<Animator>();
            bool hungryEnemy = enemy.GetComponent<EnemyController>().isHungry;
            var willHurtPlayer = (hungryEnemy ? true : false);


            if (willHurtPlayer)
            {

                Debug.Log("Hurt player");
                anim.SetTrigger("eatPlayer");
                player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //enemy.GetComponent<EnemyController>().isHungry = false;

                var ev = Schedule<PlayerDeath>();
                ev.dropBody = false;

                if(player.activeModifier == "sleep")
                {
                    enemy.GetComponent<EnemyController>().isHungry = false;
                    anim.SetTrigger("goSleep");
                }
                
                if(enemy.path)
                {
                    enemy.path.startPosition = new Vector3(0f, 0f, 0f);
                    enemy.path.endPosition = new Vector3(0f, 0f, 0f);
                }
               
                anim.SetTrigger("killedPlayer");
            }
            else
            {
                player.Bounce(bounceHeight);
            }
        }
    }
}