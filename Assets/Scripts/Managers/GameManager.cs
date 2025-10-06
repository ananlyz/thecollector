using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public LevelDatabase LevelDatabase;
    public int CurrentLevel { get; private set; }
    public int UnlockedLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SaveSystem.SaveLevel(1);
            UnlockedLevel = SaveSystem.GetUnlockedLevel();
            Debug.Log(UnlockedLevel);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CompleteLevel(string LevelName) // 支持可重写 不同的关卡可以有不同的结算
    {
        int winLevel = LevelDatabase.GetLevelKey(LevelName);
        if (winLevel+1 > UnlockedLevel)
        {
            UnlockedLevel = winLevel+1;
            SaveSystem.SaveLevel(UnlockedLevel);
        }
        SceneManager.LoadScene("LevelSelect");
    }

    public bool IsLevel(string name)
    {
        int levelKey = LevelDatabase.GetLevelKey(name);
        if (levelKey == 0) return false;
        return true;
    }

    public bool IsLevelUnLocked(string name)
    {
        int levelKey = LevelDatabase.GetLevelKey(name);
        if (levelKey <= UnlockedLevel) return true;
        return false;
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