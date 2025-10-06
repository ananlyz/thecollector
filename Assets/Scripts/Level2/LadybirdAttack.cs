using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadybirdAttack : MonoBehaviour
{
    public GameObject ladybird;
    public GameObject ladybirdShow;
    public Animator flowerAnim;
    public ImageFillTimer imageFillTimer;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                ladybirdShow.SetActive(false);
                if (flowerAnim != null)
                {
                    flowerAnim.SetTrigger("Attack");
                }
                ladybird.SetActive(true);
                playerController.TaskHurt(dir, 3f);
                if (imageFillTimer != null)
                {
                    imageFillTimer.ApplyPunish(5f);
                }
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
