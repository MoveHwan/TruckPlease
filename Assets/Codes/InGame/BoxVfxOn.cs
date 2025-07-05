using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �浹 ȿ������ ������ ��ũ��Ʈ
public class BoxVfxOn : MonoBehaviour
{

    bool vfxOn;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("�ϴ� �΋H��");
        if (!vfxOn && BoxManager.Instance.GoaledBoxes.Contains(gameObject))
        {
            ParticlePlay();
            vfxOn = true;
        }
        else if (!vfxOn)
        {
            // �ٱ��� �΋H���ų� �Ҷ� ȿ����
            AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
            vfxOn = true;
        }
    }

    // ��ƼŬ�� ȿ���� �÷���
    public void ParticlePlay()
    {
        switch (Random.Range(0,2)) 
        { 
            case 0: 
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
                break;
            case 1: 
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox2);
                break;
            case 2: 
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox3);
                InGameGoldUI.Instance.GetGold();
                break;
        }
    }
}
