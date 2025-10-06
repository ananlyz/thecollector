using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRotateMover : MonoBehaviour
{
    [Header("路径点")]
    public Vector3 posA; // 起点
    public Vector3 posB; // 中间点
    public Vector3 posC; // 终点

    [Header("速度设置")]
    public float speedAB = 2f;
    public float rSpeed = 360f;
    public float speedBC = 3f;

    [Header("旋转控制")]
    public Vector3 rotateAxis = Vector3.up;
    public float rotateAngle = 180f; 
    public bool loop = false;

    private bool moving = false;

    void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        moving = true;

        // 第一阶段：从A到B，并旋转
        yield return MoveAndRotate(posA, posB, speedAB, rotateAngle, rSpeed);

        // 第二阶段：从B到C，只移动
        yield return MoveOnly(posB, posC, speedBC);

        moving = false;

        if (loop) StartCoroutine(MoveRoutine()); // 循环播放
    }

    IEnumerator MoveAndRotate(Vector3 start, Vector3 end, float speed, float rotation, float rSpeed)
    {
        float distance = Vector3.Distance(start, end);
        float duration = distance / speed;
        float elapsed = 0f;

        float rDuration = Mathf.Abs(rotation) / rSpeed; 
        float rElapsed = 0f;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.AngleAxis(rotation, rotateAxis);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        while (elapsed < duration)
        {
            float t = rElapsed / rDuration;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            rElapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
    }

    IEnumerator MoveOnly(Vector3 start, Vector3 end, float speed)
    {
        float distance = Vector3.Distance(start, end);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
