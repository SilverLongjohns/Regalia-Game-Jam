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

        private bool isPressed = false;
        private int collisionCount = 0;

        public GameObject wall;

        // controlled object goes here

        void OnTriggerEnter2D(Collider2D collider)
        {
            this.collisionCount += 1;
            this.isPressed = true;
            wall.GetComponent<ButtonWallController>().openWall();
            this.GetComponent<SpriteRenderer>().sprite = pressed;
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            this.collisionCount -= 1;
            if (this.collisionCount <= 0)
            {
                this.isPressed = false;
                wall.GetComponent<ButtonWallController>().closeWall();
                this.GetComponent<SpriteRenderer>().sprite = unpressed;
            }
        }

        public void pressButton()
        {
            this.isPressed = true;
        }

        public void unpressButton()
        {
            this.isPressed = false;
        }

    }
}
