using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerAttributes player;

    public static GameManager Instance;
    void Awake()
    {
        Time.timeScale = 1;
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public PlayerAttributes GetPlayer() => player;

    public void NextLevel() => LoadLevel((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    public void Retry() => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    void LoadLevel(int levelIndex) => SceneManager.LoadScene(Mathf.Max(0, levelIndex));

    public void Exit() => Application.Quit();

    void Pause()
    {
        Time.timeScale = 0;
        UIManager.Instance.GetPauseScreen().SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        UIManager.Instance.GetPauseScreen().SetActive(false);
    }

    public void LevelComplete() => UIManager.Instance.GetEndLevelScreen().SetActive(true);
}
