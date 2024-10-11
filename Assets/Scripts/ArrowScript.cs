using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float destroyTime = 5.0f;

    // 생성 5초후 제거
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    // 다른 오브젝트와 충돌시 제거
    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);        
    }
}

