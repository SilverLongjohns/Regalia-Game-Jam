using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneController 
{
    // Start is called before the first frame update
    static void Start()
    {
        SceneManager.LoadScene("Level1");
    }

    public enum Scene
    {
        Level1,
        Level2
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
