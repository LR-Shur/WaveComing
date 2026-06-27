using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameStartStudio;

public class GameStart : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);

        SceneManager.LoadScene("Menu");
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene previousActiveScene, Scene newActiveScene)
    {
        SoundManager.Instance.PlayMusic(newActiveScene.name);
    }


    [RuntimeInitializeOnLoadMethod]
    public static void OnGameLoaded()
    {
        if (SceneManager.GetActiveScene().name == "GameStart")
        {
            return;
        }

        SceneManager.LoadScene("GameStart");
    }
}
