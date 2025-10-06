using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public GameObject winOj;
    public GameObject loseOj;
    public GameObject back;
    private bool isWin = false;
    private bool isLose = false;
    public Sprite winBG;
    public Sprite loseBG;
    private Image image;
    public AudioClip winClip;
    public AudioClip LoseClip;
    private void Awake()
    {
        image = GetComponent<Image>();
        var lose = loseBG;
        var win = winBG;
    }
    public void WinGame()
    {
        AudioSource.PlayClipAtPoint(winClip, transform.position, 2.0f);
        loseOj.SetActive(false);
        back.SetActive(false);
        image.sprite = winBG;
        isWin = true;
        Time.timeScale = 0f;
    }
    public void LoseGame()
    {
        AudioSource.PlayClipAtPoint(LoseClip, transform.position, 2.0f);
        winOj.SetActive(false);
        image.sprite = loseBG;
        isLose = true;
        Time.timeScale = 0f;
    }
    void Update()
    {
        if (isWin && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            GameManager.Instance.CompleteLevel(SceneManager.GetActiveScene().name);
        }

        if (isLose && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            SceneController.Instance.LoadLevel(SceneManager.GetActiveScene().name);
        }
    }
}
