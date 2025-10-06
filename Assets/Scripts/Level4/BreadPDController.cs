using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class BreadPDController : MonoBehaviour
{
    public GameObject breadWall;
    public Vector3 oriPos;
    public Vector3 tarPos;
    public float speed = 2f;

    public Vector3 oriBreadPos;
    public Vector3 tarBreadPos;
    public float bSpeed = 5f;
    public float ResumeScale = 2f;

    public void StartUp()
    {
        StartCoroutine(MoveOnly());
    }

    public void Resume()
    {
        StartCoroutine(MoveBack());
    }
    IEnumerator MoveOnly()
    {
        float distance = Vector3.Distance(oriPos, tarPos);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(oriPos, tarPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = tarPos;

        yield return MoveBread();
    }

    IEnumerator MoveBread()
    {
        breadWall.SetActive(true);
        float distance = Vector3.Distance(oriBreadPos, tarBreadPos);
        float duration = distance / bSpeed / ResumeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            breadWall.transform.position = Vector3.Lerp(oriBreadPos, tarBreadPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        breadWall.transform.position = tarBreadPos;
    }
    
    IEnumerator MoveBack()
    {
        float distance = Vector3.Distance(tarPos, oriPos);
        float duration = distance / speed / ResumeScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(tarPos, oriPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = oriPos;

        yield return BackBread();
    }

    IEnumerator BackBread()
    {

        float distance = Vector3.Distance(tarBreadPos, oriBreadPos);
        float duration = distance / bSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            breadWall.transform.position = Vector3.Lerp(tarBreadPos, oriBreadPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        breadWall.transform.position = oriBreadPos;
        breadWall.SetActive(false);
    }
}
