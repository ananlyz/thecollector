using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillTimer : MonoBehaviour
{
    public Image targetImage;
    public Image backgroundImage;
    public float duration = 60f;
    private float timer = 0f;
    public GameObject gameFlowManager;

    void Start()
    {
        if (targetImage != null && backgroundImage != null)
        {
            targetImage.fillAmount = 0f; // 从0开始显示
            backgroundImage.fillAmount = 0f;
        }
    }

    void Update()
    {
        if (targetImage == null || backgroundImage == null) return;
        if (timer >= duration)
        {
            gameFlowManager.SetActive(true);
            gameFlowManager.GetComponent<GameFlowManager>().LoseGame();
            return;
        }

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / duration);
        targetImage.fillAmount = progress;
        backgroundImage.fillAmount = progress;
    }

    public void ApplyPunish(float punishDuration)
    {
        if (targetImage == null || backgroundImage == null) return;
        timer += punishDuration;
        float progress = Mathf.Clamp01(timer / duration);
        targetImage.fillAmount = progress;
        backgroundImage.fillAmount = progress;
    }
}
