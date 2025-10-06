using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadController : MonoBehaviour
{
    public Vector3 tarPos;
    public Vector3 oriPos;
    public GameObject oriLight;
    public GameObject tarLight;
    public GameObject breadUp;
    public float delay = 10f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = tarPos;
            oriLight.SetActive(false);
            tarLight.SetActive(true);
            breadUp.GetComponent<BreadPDController>().StartUp();
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Resume());
        }
    }

    IEnumerator Resume()
    {
        yield return new WaitForSeconds(delay);
        transform.position = oriPos;
        oriLight.SetActive(true);
        tarLight.SetActive(false);
        breadUp.GetComponent<BreadPDController>().Resume();
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
    
}
