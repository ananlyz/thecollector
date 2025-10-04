using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; }
    public int UnlockedLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UnlockedLevel = SaveSystem.GetUnlockedLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel() // 支持可重写 不同的关卡可以有不同的结算
    {
        int nextLevel = CurrentLevel + 1;
        if (nextLevel > UnlockedLevel)
        {
            UnlockedLevel = nextLevel;
            SaveSystem.SaveLevel(UnlockedLevel);
        }
        SceneManager.LoadScene("LevelSelect");
    }
}

public static class SaveSystem
{
    private const string LevelKey = "UnlockedLevel";

    public static void SaveLevel(int level)
    {
        PlayerPrefs.SetInt(LevelKey, level);
        PlayerPrefs.Save();
    }

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt(LevelKey, 1); // 默认解锁第1关
    }
}