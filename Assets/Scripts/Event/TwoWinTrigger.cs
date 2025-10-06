using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoWinTrigger : MonoBehaviour
{
    private int currCount = 0;
    public int tarCount = 2;
    public GameObject gameFlowManager;
    void Awake()
    {
        currCount = 0;
    }
    void Update()
    {
        if (currCount >= tarCount)
        {
            gameFlowManager.SetActive(true);
            gameFlowManager.GetComponent<GameFlowManager>().WinGame();
        }
    }
    public void GetOneTarget()
    {
        currCount++;
    }
}
