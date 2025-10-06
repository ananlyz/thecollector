using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillSpin : MonoBehaviour
{
    public float rotateSpeed = 180f;

    void Update()
    {
        transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
    }
}
