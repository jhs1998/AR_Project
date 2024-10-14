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
        // 배틀 씬으로 전환
        SceneManager.LoadScene("BattleScene01"); 

        // 버튼 삭제
        Destroy(gameObject); // 버튼 객체 삭제

        
        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null)
        {
            mapManager.enemynum -= 1; // enemynum 감소
        }
    }
}
