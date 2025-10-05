using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BaseButton : MonoBehaviour
{
    protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // 子类重写
    protected virtual void OnClick()
    {
        Debug.Log("BaseButton 被点击");
    }

    protected virtual void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }
}
