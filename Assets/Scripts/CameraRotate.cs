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
        float ��v����Y�� = Camera.main.transform.rotation.eulerAngles.y;
        this.transform.rotation = Quaternion.Euler(0f, ��v����Y��, 0f);
    }
}
