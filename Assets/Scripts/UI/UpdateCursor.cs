using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCursor : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject GameFloeManager;

    void Update()
    {
        if(PausePanel.activeSelf || GameFloeManager.activeSelf)
        {
            CursorManager.Instance.EnableCursor(true);
        }
        else
        {
            CursorManager.Instance.EnableCursor(false);
        }
    }
}
