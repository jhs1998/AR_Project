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

    // 위도 경도 변수 
    private float latitude = 0;
    private float longitude = 0;

    public float Latitude => latitude; // 접근자
    public float Longitude => longitude; // 접근자

    private bool receiveGPS = false;
    private float waitTime = 0;

    private void Start()
    {
        StartCoroutine(GPS_On());
    }

    public IEnumerator GPS_On()
    {
        // GPS 권한 요청
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // GPS 활성화 확인
        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            yield break;
        }

        // 위치 데이터 요청
        Input.location.Start();

        // GPS 수신 초기화 대기
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // 수신 실패 시 메시지 출력
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수신 실패";
            yield break;
        }

        // 수신된 GPS 데이터를 화면에 출력
        UpdateLocationText();

        receiveGPS = true;

        // 위치 데이터 수신 시작 후 위치 정보 갱신 후 출력
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
        latitude_text.text = "위도: " + latitude.ToString();
        longitude_text.text = "경도: " + longitude.ToString();
    }

    private void OnDisable()
    {
        Input.location.Stop(); // 비활성화 시 위치 서비스 중지
    }
}
