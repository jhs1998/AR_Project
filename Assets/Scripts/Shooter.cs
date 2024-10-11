using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Shooter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // 발사체 프리펩
    [SerializeField] GameObject shootPrefab;
    
    // 버튼의 눌림 유무
    public bool isButton = false;

    // 최대 발사체 속도
    public float maxShootSpeed = 20f;
    // 최대 충전 시간
    public float maxchargeTime = 3f;
    // 충전 시간
    public float chargeTime = 0f;
    

    private void Update()
    {
        if (isButton)
        {
            // 누르는 시간만큼 충전
            chargeTime += Time.deltaTime;
            // 최대 충전 제한
            chargeTime = Mathf.Clamp(chargeTime, 0, maxchargeTime);
        }
    }    

    // 버튼이 눌릴 때 실행
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("버튼을 누름");
        isButton = true;
        chargeTime = 0f;
    }

    // 버튼을 뗄 때 실행
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isButton)
        {
            Shoot();
        }
        isButton = false;
    }

    // 버튼이 눌린 상태에서 커서가 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isButton)
        {
            Shoot();
        }
        isButton = false;
    }

    public void Shoot()
    {
        float shootSpeed = Mathf.Lerp(0, maxShootSpeed, chargeTime / maxchargeTime);

        GameObject ball = Instantiate(shootPrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        rigidbody.velocity = shootSpeed * Camera.main.transform.forward;
    }
}
