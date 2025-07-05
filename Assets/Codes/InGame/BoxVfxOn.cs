using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이제 충돌 효과음만 나오는 스크립트
public class BoxVfxOn : MonoBehaviour
{

    bool vfxOn;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("일단 부딫힘");
        if (!vfxOn && BoxManager.Instance.GoaledBoxes.Contains(gameObject))
        {
            ParticlePlay();
            vfxOn = true;
        }
        else if (!vfxOn)
        {
            // 바깥에 부딫히거나 할때 효과음
            AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
            vfxOn = true;
        }
    }

    // 파티클과 효과음 플레이
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
