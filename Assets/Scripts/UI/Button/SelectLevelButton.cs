using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelButton : BaseButton
{
    public int levelKey = 1;
    public LevelDatabase LevelDatabase;

    protected override void OnClick()
    {
        base.OnClick();
        string sceneName = LevelDatabase.GetSceneName(levelKey);
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneController.Instance.LoadLevel(sceneName);
        }
        else
        {
            Debug.LogError($"关卡 {levelKey} 的场景名称未找到！");
        }
    }
}
