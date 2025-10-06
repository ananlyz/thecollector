using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTriiger : MonoBehaviour
{
    public BoxCollider2D upPlatform;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), upPlatform, true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), upPlatform, false);
        }
    }

    

    
}
