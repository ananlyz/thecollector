using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject Panel;

    void Update()
    {
        if (Input.anyKey)
        {
            Panel.SetActive(false);
        }
    }

    public void quit()
    {
        Application.Quit();
    }
}
