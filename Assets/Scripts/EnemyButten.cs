using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyButton : MonoBehaviour
{
    private string battleSceneName; // ��ȯ�� ��Ʋ �� �̸�
    private int enemyIndex;

    public void SetBattleScene(string sceneName)
    {
        battleSceneName = sceneName;

        // ��ư Ŭ�� �̺�Ʈ �߰�
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnEnemyButtonClicked);
        }
    }

    private void OnEnemyButtonClicked()
    {
        // ��Ʋ ������ ��ȯ
        SceneManager.LoadScene(battleSceneName);
    }
}
