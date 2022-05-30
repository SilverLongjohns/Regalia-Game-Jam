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
                GameObject.Find(persistentObjectName + "/LoadPoint").transform.position = this.gameObject.transform.GetChild(0).transform.position;
                
                // Turn off this scene's saved objects
                string oldSceneName = this.gameObject.scene.name + "Objects";
                for(int i = 0; i < GameObject.Find(oldSceneName).transform.childCount; i++)
                {
                    GameObject.Find(oldSceneName).transform.GetChild(i).gameObject.SetActive(false);
                }
                

                // Load new scene and turn on objects
                SceneManager.LoadScene(newScene);

                string newSceneName = newScene + "Objects";
                try
                {
                    for (int i = 0; i < GameObject.Find(newSceneName).transform.childCount; i++)
                    {
                        GameObject.Find(newSceneName).transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                catch
                {
                    Debug.Log("first-time load");
                }

            }
        }
    }
}