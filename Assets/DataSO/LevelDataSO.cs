using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "GameLevel/LevelDatabase")]
public class LevelDatabase : ScriptableObject
{
    [System.Serializable]
    public class LevelEntry
    {
        public int key;         
        public string sceneName; 
    }

 
    public List<LevelEntry> levels = new List<LevelEntry>();

   
    public string GetSceneName(int key)
    {
        foreach (var entry in levels)
        {
            if (entry.key == key)
                return entry.sceneName;
        }
        return null;
    }

    public void AddLevel(int key, string sceneName)
    {
        foreach (var entry in levels)
        {
            if (entry.key == key)
            {
                entry.sceneName = sceneName;
                return;
            }
        }
        levels.Add(new LevelEntry { key = key, sceneName = sceneName });
    }
}
