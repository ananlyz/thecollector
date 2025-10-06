using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("鼠标图标设置")]
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
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

    /// <summary>
    /// 启用或禁用鼠标
    /// </summary>
    public void EnableCursor(bool enable)
    {
        if (enable)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (cursorTexture != null)
                Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 仅更新鼠标图标（不改变显示状态）
    /// </summary>
    public void UpdateCursorIcon(Texture2D newIcon, Vector2 newHotspot)
    {
        cursorTexture = newIcon;
        hotSpot = newHotspot;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "LevelSelect")
        {
            EnableCursor(true);
        }
    }
}
