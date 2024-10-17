using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyButton : MonoBehaviour
{
    private string battleSceneName; // 전환할 배틀 씬 이름
    private int enemyIndex;

    public void SetBattleScene(string sceneName)
    {
        battleSceneName = sceneName;

        // 버튼 클릭 이벤트 추가
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnEnemyButtonClicked);
        }
    }

    private void OnEnemyButtonClicked()
    {
        // 배틀 씬으로 전환
        SceneManager.LoadScene(battleSceneName);
    }
}
