using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraIntroController : MonoBehaviour
{
    public CinemachineVirtualCamera overviewCam;
    public CinemachineVirtualCamera followCam;
    public float introWait = 1.2f;     // 全图展示时间
    public float zoomDuration = 1.0f;  

    public float overviewOrtho = 10.7f;  
    public float gameplayOrtho = 9f;

    public GameObject player;
    public GameObject progressBar;

    void Start()
    {
        overviewCam.gameObject.SetActive(true);
        followCam.gameObject.SetActive(true);

        overviewCam.Priority = 20;
        followCam.Priority = 10;

        overviewCam.m_Lens.OrthographicSize = overviewOrtho;
        followCam.m_Lens.OrthographicSize = gameplayOrtho;

        player.GetComponent<PlayerController>().SetPlayerState(true);
        progressBar.SetActive(false);
        StartCoroutine(IntroSequence());
        
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(introWait);

        float t = 0f;
        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float s = Mathf.Lerp(overviewOrtho, gameplayOrtho, t / zoomDuration);
            overviewCam.m_Lens.OrthographicSize = s;
            yield return null;
        }
        overviewCam.m_Lens.OrthographicSize = gameplayOrtho;

        followCam.Priority = 30;
        player.GetComponent<PlayerController>().SetPlayerState(false);
        progressBar.SetActive(true);
    }
}
