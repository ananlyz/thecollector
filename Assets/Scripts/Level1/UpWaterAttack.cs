using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpWaterAttack : MonoBehaviour
{
    public ImageFillTimer imageFillTimer;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                playerController.TaskHurt(dir, 3f);
                if (imageFillTimer != null)
                {
                    imageFillTimer.ApplyPunish(5f);
                }
            }
        }
    }
}
