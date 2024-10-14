using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyButton : MonoBehaviour
{
    private int enemyIndex; 

    public void Initialize(int index)
    {
        enemyIndex = index; 
    }

    public void OnClick()
    {
        // ��Ʋ ������ ��ȯ
        SceneManager.LoadScene("BattleScene01"); 

        // ��ư ����
        Destroy(gameObject); // ��ư ��ü ����

        
        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            mapManager.enemynum -= 1; // enemynum ����
        }
    }
}
