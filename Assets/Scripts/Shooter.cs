using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class Shooter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    // �߻�ü ������
    [SerializeField] GameObject shootPrefab;
    // ���� �߷��� �ΰ����� �־��ٰ�
    [SerializeField] GameObject monsterPrefab;

    [SerializeField] ARRaycastManager raycastManager;
    
    // ���� ��ȯ ����
    public bool ismonster = false;

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

    public void SponMonster()
    {
        Ray ray = new Ray();
        ray.origin = Camera.main.transform.position;
        ray.direction = Camera.main.transform.forward;

        List<ARRaycastHit> hits = new List<ARRaycastHit> ();
        raycastManager.Raycast(ray, hits);

        if (ismonster != true && raycastManager.Raycast(ray, hits))
        {
            Instantiate(monsterPrefab, hits[0].pose.position, hits[0].pose.rotation);
            ismonster = true;
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
