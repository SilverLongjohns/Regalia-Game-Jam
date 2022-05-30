using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a SceneTransition to move player between scenes.
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] private string newScene;
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredSceneTransition>();
                //ev.newZone = newScene;
                // Save the correct transition point to enter from next when coming back
                // IE if the player leaves the current room and re-enters, this will make it so they start at the place they left from
                string persistentObjectName = GameObject.Find("GameController").GetComponent<GameController>().getPersistentObjectName();
                Debug.Log(GameObject.Find(persistentObjectName + "/LoadPoint").transform.position);
                Debug.Log(this.gameObject.transform.GetChild(0).transform);
                GameObject.Find(persistentObjectName + "/LoadPoint").transform.position = this.gameObject.transform.GetChild(0).transform.position;
                Debug.Log(GameObject.Find(persistentObjectName + "/LoadPoint").transform.position);
                SceneManager.LoadScene(newScene);
            }
        }
    }
}