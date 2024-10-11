using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Shooter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // �߻�ü ������
    [SerializeField] GameObject shootPrefab;
    
    // ��ư�� ���� ����
    public bool isButton = false;

    // �ִ� �߻�ü �ӵ�
    public float maxShootSpeed = 20f;
    // �ִ� ���� �ð�
    public float maxchargeTime = 3f;
    // ���� �ð�
    public float chargeTime = 0f;
    

    private void Update()
    {
        if (isButton)
        {
            // ������ �ð���ŭ ����
            chargeTime += Time.deltaTime;
            // �ִ� ���� ����
            chargeTime = Mathf.Clamp(chargeTime, 0, maxchargeTime);
        }
    }    

    // ��ư�� ���� �� ����
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("��ư�� ����");
        isButton = true;
        chargeTime = 0f;
    }

    // ��ư�� �� �� ����
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isButton)
        {
            Shoot();
        }
        isButton = false;
    }

    // ��ư�� ���� ���¿��� Ŀ���� ����� ��
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
