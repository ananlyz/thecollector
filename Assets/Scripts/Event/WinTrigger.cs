using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public GameObject gameFlowManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameFlowManager.SetActive(true);
            gameFlowManager.GetComponent<GameFlowManager>().WinGame();
        }
    }
}
