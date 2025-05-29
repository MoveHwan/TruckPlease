using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVFXCheck : MonoBehaviour
{
    public GameObject groundVFXPrefab; // ��ƼŬ �������� �ν����Ϳ��� ����

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�ε�ħ");

        if (other.CompareTag("Box"))
        {
            Debug.Log(other.name);
            //BoxVfxOn boxVfxOn = other.GetComponent<BoxVfxOn>();
            //if (boxVfxOn != null) 
            //{
            //    boxVfxOn.ParticlePlay();
            //}
            // �浹 ��ġ�� �������� ��ƼŬ ��ȯ
            RotationBox rotationBox = other.GetComponent<RotationBox>();
            if (!rotationBox.vfxDone)
            {
                rotationBox.vfxDone = true;
                Vector3 spawnPosition = other.ClosestPoint(transform.position); // �浹 ���� �ٻ�ġ
                Instantiate(groundVFXPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
