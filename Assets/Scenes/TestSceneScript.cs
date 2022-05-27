using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

public class TestSceneScript : MonoBehaviour
{
    [SerializeField] private int counter = 0;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private string newSceneName = null;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void incrementCounter()
    {
        counter += 1;
        counterText.text = counter.ToString();
    }

    public void SwapScene()
    {
        SceneManager.LoadScene(newSceneName);
    }
}
