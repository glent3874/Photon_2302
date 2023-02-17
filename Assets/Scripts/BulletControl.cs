using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private void Update()
    {
        this.transform.Translate(0f, 0f, speed * Time.deltaTime, Space.Self);    
    }

    public void 設定目標(Vector3 目標點)
    {
        this.transform.LookAt(目標點);
    }

    public void 補償時間(float _lag多久)
    {
        this.transform.Translate(0f, 0f, speed * _lag多久, Space.Self);
    }
}
