using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public static CameraRotate instance = null;
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float 攝影機的Y值 = Camera.main.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(0f, 攝影機的Y值, 0f);
    }
}
