using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.ARSubsystems;

public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage; // 인스펙터에서 할당

    [Header("Map SET")]
    public string strBaseURL = "https://maps.googleapis.com/maps/api/staticmap?"; // 지도 API의 구글 url입력
    public int zoom = 14; // 지도 확대 수준
    public int mapWidth = 640; // 지도 너비 (픽셀)
    public int mapHeight = 640; // 지도 높이 (픽셀)
    public string strAPIKey = ""; // API 키
    public GameObject characterPrefab; // 캐릭터 프리펩

    private GPS_Manager gpsManager; // GPS_Manager 인스턴스를 저장할 변수
    private double save_latitude = 0; // 이전 위도
    private double save_longitude = 0; // 이전 경도

    private void Start()
    {
        mapWidth = Screen.width;
        mapHeight = Screen.height;

        if (mapRawImage == null)
        {
            Debug.LogError("mapRawImage가 인스펙터에서 할당되지 않았습니다.");
            return;
        }
        gpsManager = FindObjectOfType<GPS_Manager>(); // GPS_Manager 인스턴스를 찾음

        if (gpsManager == null)
        {
        Debug.LogError("GPS_Manager 인스턴스를 찾을 수 없습니다. 올바르게 설정했는지 확인하세요.");
        return; // GPS_Manager가 없으면 실행안함
        }

        StartCoroutine(WaitForSecond());
    }

    IEnumerator WaitForSecond()
    {
        while (true)
        {
            double latitude = gpsManager.Latitude; // GPS_Manager에서 위도를 가져옴
            double longitude = gpsManager.Longitude; // GPS_Manager에서 경도를 가져옴

            // 위도와 경도가 변경 확인
            if (save_latitude != latitude || save_longitude != longitude)
            {
                save_latitude = latitude;
                save_longitude = longitude;
                yield return LoadMap(latitude, longitude);
            }
            yield return new WaitForSeconds(3f); // 3초 대기
        }
    }

    IEnumerator LoadMap(double latitude, double longitude)
    {
        
        // 지도 API 요청 URL 제작
        string url = $"https://maps.googleapis.com/maps/api/staticmap?center={latitude},{longitude}&zoom={zoom}&size={mapWidth}x{mapHeight}&key={strAPIKey}";

        Debug.Log("URL : " + url);

        url = UnityWebRequest.UnEscapeURL(url); // URL 인코딩을 해제
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest(); // 요청 수행

            // 요청 결과 확인
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("맵 로드 실패: " + req.error);
            }
            else
            {
                // 맵을 이미지에 적용
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                if (texture != null)
                {
                    Debug.Log("Texture successfully downloaded and applied.");
                    mapRawImage.texture = texture;

                    // 캐릭터 오브젝트를 위치에 배치
                    PlaceCharacter(latitude, longitude);
                }
                if (texture != null)
                {
                    Debug.Log("Texture successfully downloaded and applied.");
                    if (mapRawImage != null)
                    {
                        mapRawImage.texture = texture;
                    }
                    else
                    {
                        Debug.LogError("mapRawImage is null.");
                    }
                }
                else
                {
                    Debug.LogError("다운로드한 텍스처가 null입니다.");
                }
            }
        }

    }

    Vector2 LatLongToScreenPos(double latitude, double longitude)
    {
        // 지도에서 중앙 위도, 경도
        double mapCenterLat = save_latitude;
        double mapCenterLon = save_longitude;

        // 화면 크기
        float width = mapWidth;
        float height = mapHeight;

        // 위도와 경도 차이 계산
        double latDiff = latitude - mapCenterLat;
        double lonDiff = longitude - mapCenterLon;

        // 픽셀 좌표로 변환
        float xPos = (float)((lonDiff * (width / 360)) + (width / 2));
        float yPos = (float)((latDiff * (height / 180)) + (height / 2));

        return new Vector2(xPos, yPos);
    }
}
