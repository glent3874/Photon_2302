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

    public void �]�w�ؼ�(Vector3 �ؼ��I)
    {
        this.transform.LookAt(�ؼ��I);
    }

    public void ���v�ɶ�(float _lag�h�[)
    {
        this.transform.Translate(0f, 0f, speed * _lag�h�[, Space.Self);
    }
}
