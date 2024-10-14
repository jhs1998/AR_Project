using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeScript : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public float monsterdowntime = 2.0f;

    public float monsterHP = 3;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            switch (monsterHP)
            {
                case 3:
                    monsterHP -= 1;
                    // GetHit �ִϸ��̼� ����
                    animator.SetTrigger("GetHit");
                    break;
                case 2:
                    monsterHP -= 1;
                    // GetHit �ִϸ��̼� ����
                    animator.SetTrigger("GetHit");
                    break;
                case 1:
                    monsterHP -= 1;
                    // GetHit �ִϸ��̼� ����
                    animator.SetTrigger("GetHit");
                    // Dizzy �ִϸ��̼� ����
                    animator.SetTrigger("Dizzy");
                    break;
                case 0:
                    // GetHit �ִϸ��̼� ����
                    animator.SetTrigger("GetHit");
                    // Die �ִϸ��̼� ����
                    animator.SetTrigger("Die");
                    SceneManager.LoadScene("MapScene");
                    Destroy(gameObject, monsterdowntime);
                    break;
            }
        }
    }
}
