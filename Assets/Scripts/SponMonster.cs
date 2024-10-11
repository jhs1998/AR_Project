using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SponMonster : MonoBehaviour
{
    // 몬스터 중류는 두가지를 넣어줄것
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] ARRaycastManager raycastManager;
    [SerializeField] ARPlaneManager planeManager;
    // 몬스터 소환 유무
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
            // 몬스터 180도 회전
            Quaternion adjustedRotation = hitPose.rotation * Quaternion.Euler(0, 180, 0);
            Instantiate(monsterPrefab, hitPose.position, hitPose.rotation);
            ismonster = true;
        }
    }
}
