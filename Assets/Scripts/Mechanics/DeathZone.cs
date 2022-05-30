using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// DeathZone components mark a collider which will schedule a
    /// PlayerEnteredDeathZone event when the player enters the trigger.
    /// </summary>
    public class DeathZone : MonoBehaviour
    {
        
        void OnCollisionEnter2D(Collider2D collider)
        {

            if (collider.tag == "Player")
            {
                var p = collider.gameObject.GetComponent<PlayerController>();

                //ContactPoint2D[] collisionPoint = new ContactPoint2D[1];
                //collider.GetContacts(collisionPoint);

                //Debug.Log(collisionPoint[0].point);



                if (p != null)
                {
                    var ev = Schedule<PlayerEnteredDeathZone>();
                    ev.deathzone = this;
                }
            }
        }
    }
};