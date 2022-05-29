using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private string persistentObjectName;

        void Awake()
        {
            persistentObjectName = getPersistentObjectName();

            if (GameObject.Find(getPersistentObjectName()) == null)
            {
                GameObject persistentObject = new GameObject(getPersistentObjectName());
                DontDestroyOnLoad(persistentObject);

                GameObject deadBodies = new GameObject("DeadBodies");
                DontDestroyOnLoad(deadBodies);
                deadBodies.transform.SetParent(persistentObject.transform);

                GameObject loadPoint = new GameObject("LoadPoint");
                DontDestroyOnLoad(loadPoint);
                loadPoint.transform.position = GameObject.Find("StartingPoint").transform.position;
                loadPoint.transform.SetParent(persistentObject.transform);
            }
            model.player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
            model.player.gameObject.transform.position = GameObject.Find(persistentObjectName + "/LoadPoint").transform.position;
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt = GameObject.FindGameObjectsWithTag("Player")[0].transform;
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }

        void Start()
        {
            loadAssets();
        }

        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }

        public string getPersistentObjectName()
        {
            return gameObject.scene.name + "Objects";
        }

        public void loadAssets()
        {
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().LookAt = GameObject.FindGameObjectsWithTag("Player")[0].transform;
            GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        }
    }
}