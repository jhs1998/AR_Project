using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class GPS_Manager : MonoBehaviour
{
    // 텍스트 ui qustn
    public Text latitude_text;
    public Text longitude_text;

    public float maxWaitTime = 10.0f;
    public float resendTime = 1.0f;

    // 위도 경도 변수 
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

        //위치 데이터 요청 -> 수신 대기
        Input.location.Start();

        // GPS 수신 상태가 초기 상태에서 일정 시간 동안 대기
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // 수신 실패 시 수신이 실패됬다는 것을 출력
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수신 실패";
        }

        // 응답 대기시간 초과시 수신이 없다면 시간 초과되었음을 출력
        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";
        }

        //수신된 GPS 데이터를 화면에 출력
        LocationInfo li = Input.location.lastData;
        latitude = li.latitude;
        longitude = li.longitude;
        latitude_text.text = "위도: " + latitude.ToString();
        longitude_text.text = "경도: " + longitude.ToString();

        // 위치 정보 수신 시작 체크
        receiveGPS = true;

        // 위치 데이터 수신 시작 후 위치 정보 갱신 후 출력
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;
            latitude_text.text = "위도: " + latitude.ToString();
            longitude_text.text = "경도: " + longitude.ToString();
        }
    }
}
