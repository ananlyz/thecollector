using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    void Awake()
    {
        // ehanshi
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Input.anyKeyDown)
            {
                LoadLevelSelect();
            }
        }
    }

    #region 场景跳转函数

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
   
    public void LoadLevel(string levelSceneName)
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
