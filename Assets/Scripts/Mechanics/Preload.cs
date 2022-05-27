using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Preload : MonoBehaviour
{

    public GameObject player;

    // Start is called the first time
    void Awake()
    {
        DontDestroyOnLoad(Instantiate(player));
        SceneManager.LoadScene(1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
