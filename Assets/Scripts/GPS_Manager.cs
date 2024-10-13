using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPS_Manager : MonoBehaviour
{
    // �ؽ�Ʈ ui qustn
    public Text latitude_text;
    public Text longitude_text;

    public float maxWaitTime = 10.0f;
    public float resendTime = 1.0f;

    // ���� �浵 ���� 
    public float latitude = 0;
    public float longitude = 0;

    bool receiveGPS = false;
    float waitTime = 0;

    private void Start()
    {
        StartCoroutine(GPS_On());
    }

    public IEnumerator GPS_On()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);

            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        if(!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            yield break;
        }

        //��ġ ������ ��û -> ���� ���
        Input.location.Start();

        // GPS ���� ���°� �ʱ� ���¿��� ���� �ð� ���� ���
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // ���� ���� �� ������ ���Љ�ٴ� ���� ���
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "��ġ ���� ���� ����";
            longitude_text.text = "��ġ ���� ���� ����";
        }

        // ���� ���ð� �ʰ��� ������ ���ٸ� �ð� �ʰ��Ǿ����� ���
        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "���� ��� �ð� �ʰ�";
            longitude_text.text = "���� ��� �ð� �ʰ�";
        }

        //���ŵ� GPS �����͸� ȭ�鿡 ���
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "����: " + latitude.ToString();
        longitude_text.text = "�浵: " + longitude.ToString();

        // ��ġ ���� ���� ���� üũ
        receiveGPS = true;

        // ��ġ ������ ���� ���� �� ��ġ ���� ���� �� ���
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;
            latitude_text.text = "����: " + latitude.ToString();
            longitude_text.text = "�浵: " + longitude.ToString();
        }
    }
}
