using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SponMonster : MonoBehaviour
{
    // ���� �߷��� �ΰ����� �־��ٰ�
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] ARPlaneManager planeManager;
    // ���� ��ȯ ����
    public bool ismonster = false;

    private void Update()
    {
        MonsterSpon();
    }

    public void MonsterSpon()
    {

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (ismonster != true && raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            // ���� 180�� ȸ��
            Quaternion adjustedRotation = hitPose.rotation * Quaternion.Euler(0, 180, 0);
            Instantiate(monsterPrefab, hitPose.position, hitPose.rotation);
            ismonster = true;
        }
    }
}
