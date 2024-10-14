using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR.ARSubsystems;

public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage; // �ν����Ϳ��� �Ҵ�

    [Header("Map SET")]
    public string strBaseURL = "https://maps.googleapis.com/maps/api/staticmap?"; // ���� API�� ���� url�Է�
    public int zoom = 14; // ���� Ȯ�� ����
    public int mapWidth = 640; // ���� �ʺ� (�ȼ�)
    public int mapHeight = 640; // ���� ���� (�ȼ�)
    public string strAPIKey = ""; // API Ű
    public GameObject characterPrefab; // ĳ���� ������

    private GPS_Manager gpsManager; // GPS_Manager �ν��Ͻ��� ������ ����
    private double save_latitude = 0; // ���� ����
    private double save_longitude = 0; // ���� �浵

    private void Start()
    {
        mapWidth = Screen.width;
        mapHeight = Screen.height;

        if (mapRawImage == null)
        {
            Debug.LogError("mapRawImage�� �ν����Ϳ��� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
        gpsManager = FindObjectOfType<GPS_Manager>(); // GPS_Manager �ν��Ͻ��� ã��

        if (gpsManager == null)
        {
        Debug.LogError("GPS_Manager �ν��Ͻ��� ã�� �� �����ϴ�. �ùٸ��� �����ߴ��� Ȯ���ϼ���.");
        return; // GPS_Manager�� ������ �������
        }

        StartCoroutine(WaitForSecond());
    }

    IEnumerator WaitForSecond()
    {
        while (true)
        {
            double latitude = gpsManager.Latitude; // GPS_Manager���� ������ ������
            double longitude = gpsManager.Longitude; // GPS_Manager���� �浵�� ������

            // ������ �浵�� ���� Ȯ��
            if (save_latitude != latitude || save_longitude != longitude)
            {
                save_latitude = latitude;
                save_longitude = longitude;
                yield return LoadMap(latitude, longitude);
            }
            yield return new WaitForSeconds(3f); // 3�� ���
        }
    }

    IEnumerator LoadMap(double latitude, double longitude)
    {
        
        // ���� API ��û URL ����
        string url = $"https://maps.googleapis.com/maps/api/staticmap?center={latitude},{longitude}&zoom={zoom}&size={mapWidth}x{mapHeight}&key={strAPIKey}";

        Debug.Log("URL : " + url);

        url = UnityWebRequest.UnEscapeURL(url); // URL ���ڵ��� ����
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest(); // ��û ����

            // ��û ��� Ȯ��
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("�� �ε� ����: " + req.error);
            }
            else
            {
                // ���� �̹����� ����
                Texture2D texture = DownloadHandlerTexture.GetContent(req);
                if (texture != null)
                {
                    Debug.Log("Texture successfully downloaded and applied.");
                    mapRawImage.texture = texture;

                    // ĳ���� ������Ʈ�� ��ġ�� ��ġ
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
                    Debug.LogError("�ٿ�ε��� �ؽ�ó�� null�Դϴ�.");
                }
            }
        }

    }

    Vector2 LatLongToScreenPos(double latitude, double longitude)
    {
        // �������� �߾� ����, �浵
        double mapCenterLat = save_latitude;
        double mapCenterLon = save_longitude;

        // ȭ�� ũ��
        float width = mapWidth;
        float height = mapHeight;

        // ������ �浵 ���� ���
        double latDiff = latitude - mapCenterLat;
        double lonDiff = longitude - mapCenterLon;

        // �ȼ� ��ǥ�� ��ȯ
        float xPos = (float)((lonDiff * (width / 360)) + (width / 2));
        float yPos = (float)((latDiff * (height / 180)) + (height / 2));

        return new Vector2(xPos, yPos);
    }
}
