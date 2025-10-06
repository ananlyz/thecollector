using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public bool isTwoRequire = false;
    public TwoWinTrigger twoWinTrigger;
    public GameObject gameFlowManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isTwoRequire)
            {
                twoWinTrigger.GetOneTarget();
                Destroy(gameObject);
                return;
            }
            gameFlowManager.SetActive(true);
            gameFlowManager.GetComponent<GameFlowManager>().WinGame();
        }
    }
}
