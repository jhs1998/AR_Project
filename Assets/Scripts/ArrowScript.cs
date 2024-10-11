using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float destroyTime = 5.0f;

    // ���� 5���� ����
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    // �ٸ� ������Ʈ�� �浹�� ����
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);        
    }
}

