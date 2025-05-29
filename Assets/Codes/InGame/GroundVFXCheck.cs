using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVFXCheck : MonoBehaviour
{
    public GameObject groundVFXPrefab; // 파티클 프리팹을 인스펙터에서 지정

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("부디침");

        if (other.CompareTag("Box"))
        {
            Debug.Log(other.name);
            //BoxVfxOn boxVfxOn = other.GetComponent<BoxVfxOn>();
            //if (boxVfxOn != null) 
            //{
            //    boxVfxOn.ParticlePlay();
            //}
            // 충돌 위치를 기준으로 파티클 소환
            RotationBox rotationBox = other.GetComponent<RotationBox>();
            if (!rotationBox.vfxDone)
            {
                rotationBox.vfxDone = true;
                Vector3 spawnPosition = other.ClosestPoint(transform.position); // 충돌 지점 근사치
                Instantiate(groundVFXPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
