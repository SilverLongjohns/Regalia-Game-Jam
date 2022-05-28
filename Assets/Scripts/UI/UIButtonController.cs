using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
    [SerializeField] private string newSceneName = null;

    public void SwapScene()
    {
        SceneManager.LoadScene(newSceneName);
    }
}
