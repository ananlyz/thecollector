using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpWaterAttack : MonoBehaviour
{
    public float showDuration = 2f;
    public GameObject upWater;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            upWater.SetActive(true);
            ShowObjectForSeconds();
        }
    }

    public void ShowObjectForSeconds()
    {
        StartCoroutine(ShowObjectRoutine());
    }

    IEnumerator ShowObjectRoutine()
    {
        GetComponent<Collider2D>().enabled = false; 
        yield return new WaitForSeconds(showDuration);
        upWater.SetActive(false);
        
    }
}
