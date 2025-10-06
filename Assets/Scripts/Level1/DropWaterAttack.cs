using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWaterAttack : MonoBehaviour
{
    public AudioClip dropWaterSFX;
    void Start()
    {
        AudioSource.PlayClipAtPoint(dropWaterSFX, transform.position, 2.0f);
    }
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
                Destroy(gameObject);
            }
        }
    }
}
