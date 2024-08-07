using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [HideInInspector] public VideoPlayer videoPlayer;
    [SerializeField] GameObject Panel;
    [SerializeField] private GameObject levelsPanel;
    [HideInInspector] public Audio _audioManager;


    private void Start()
    {

        _audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();

    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        _audioManager.PlaySFX(_audioManager.ButtonClick);

    }

    public void Setting()
    {
        Panel.SetActive(true);
        _audioManager.PlaySFX(_audioManager.IconClick);


    }

    public void BackToMenu()
    {
        Panel.SetActive(false);
        _audioManager.PlaySFX(_audioManager.ButtonClick);


    }

    public void PauseGame()
    {
        Panel.SetActive(true);
        _audioManager.PlaySFX(_audioManager.IconClick);
        Time.timeScale = 0f;

    }

    public void ResumeGame()
    {
        Panel.SetActive(false);
        _audioManager.PlaySFX(_audioManager.ButtonClick);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _audioManager.PlaySFX(_audioManager.ButtonClick);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        _audioManager.PlaySFX(_audioManager.ButtonClick);
        Time.timeScale = 1f;
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void Levels()
    {
        levelsPanel.SetActive(true);
    }
}
