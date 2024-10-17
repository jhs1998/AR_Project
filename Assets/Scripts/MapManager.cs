using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // SceneManager ����� ���� �߰�

public class MapManager : MonoBehaviour
{
    public RawImage mapRawImage; // �ν����Ϳ��� �Ҵ�

    [Header("Map SET")]
    public string strBaseURL = "https://maps.googleapis.com/maps/api/staticmap?"; // ���� API�� ���� url�Է�
    public int zoom = 14; // ���� Ȯ�� ����
    public int mapWidth = 640; // ���� �ʺ� (�ȼ�)
    public int mapHeight = 640; // ���� ���� (�ȼ�)
    public string strAPIKey = ""; // API Ű
    public GameObject enemyButtonPrefab; // �� ���� ��ư ������

    private GPS_Manager gpsManager; // GPS_Manager �ν��Ͻ��� ������ ����
    private double save_latitude = 0; // ���� ����
    private double save_longitude = 0; // ���� �浵
    public int enemynum;

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
                    if (mapRawImage != null)
                    {
                        mapRawImage.texture = texture;

                        // ���� �� ��ư ��ġ
                        SpawnRandomEnemyButtons(3); // 3���� �� ��ư�� �����ϰ� ��ġ
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

    void SpawnRandomEnemyButtons(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (enemynum < 3) 
            {
                enemynum += 1;

                // ���� ��ġ ����
                float randomX = Random.Range(0, mapWidth);
                float randomY = Random.Range(0, mapHeight);

                // ��ư ����
                GameObject enemyButton = Instantiate(enemyButtonPrefab);
                enemyButton.transform.SetParent(mapRawImage.transform, false); 
                enemyButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX - (mapWidth / 2), randomY - (mapHeight / 2)); // ���� ��ġ ����

                // EnemyButton ��ũ��Ʈ �ʱ�ȭ
                EnemyButton buttonScript = enemyButton.AddComponent<EnemyButton>();
                buttonScript.SetBattleScene("BattleScene01");
            }
        }
    }
}
