using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class ButtonController : MonoBehaviour
    {
        public Sprite unpressed;
        public Sprite pressed;

        private int collisionCount = 0;

        private GameObject wall;

        // controlled object goes here

        void Start()
        {
            this.wall = this.gameObject.transform.GetChild(0).gameObject;
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            this.collisionCount += 1;
            wall.GetComponent<ButtonWallController>().openWall();
            this.GetComponent<SpriteRenderer>().sprite = pressed;
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            this.collisionCount -= 1;
            if (this.collisionCount <= 0)
            {
                wall.GetComponent<ButtonWallController>().closeWall();
                this.GetComponent<SpriteRenderer>().sprite = unpressed;
            }
        }
    }
}
