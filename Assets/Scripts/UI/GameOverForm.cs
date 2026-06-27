using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverForm : MonoBehaviour
{
    // TODO Tween
   [SerializeField] private GameObject masks;
   [SerializeField] private Button menuButton;
   [SerializeField] private Button restartButton;
   [SerializeField] private Button quitButton;

   private void Awake()
   {
        masks.SetActive(false);

        menuButton.onClick.AddListener(OnMenuButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
   }

   private void OnEnable()
   {
        GameEvents.GameOver += GameOver;
   }



   private void OnMenuButtonClick()
   {
        // change scene
        SceneManager.LoadScene("Menu");
   }

   private void OnRestartButtonClick()
   {
        SceneManager.LoadScene(GameManager.Instance.SelectedMapScene);
   }

   private void OnQuitButtonClick()
   {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
   }

   private void GameOver()
   {
        masks.SetActive(true);
   }

   private void OnDisable()
   {
        GameEvents.GameOver -= GameOver;
   }
}
