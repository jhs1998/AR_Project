using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPS_Manager : MonoBehaviour
{
    public Text latitude_text;
    public Text longitude_text;

    public float maxWaitTime = 10.0f;
    public float resendTime = 1.0f;

    // ���� �浵 ���� 
    private float latitude = 0;
    private float longitude = 0;

    public float Latitude => latitude; // ������
    public float Longitude => longitude; // ������

    private bool receiveGPS = false;
    private float waitTime = 0;

    private void Start()
    {
        StartCoroutine(GPS_On());
    }

    public IEnumerator GPS_On()
    {
        // GPS ���� ��û
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // GPS Ȱ��ȭ Ȯ��
        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            yield break;
        }

        // ��ġ ������ ��û
        Input.location.Start();

        // GPS ���� �ʱ�ȭ ���
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // ���� ���� �� �޽��� ���
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "��ġ ���� ���� ����";
            longitude_text.text = "��ġ ���� ���� ����";
            yield break;
        }

        // ���ŵ� GPS �����͸� ȭ�鿡 ���
        UpdateLocationText();

        receiveGPS = true;

        // ��ġ ������ ���� ���� �� ��ġ ���� ���� �� ���
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);
            UpdateLocationText();
        }
    }

    private void UpdateLocationText()
    {
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "����: " + latitude.ToString();
        longitude_text.text = "�浵: " + longitude.ToString();
    }

    private void OnDisable()
    {
        Input.location.Stop(); // ��Ȱ��ȭ �� ��ġ ���� ����
    }
}
