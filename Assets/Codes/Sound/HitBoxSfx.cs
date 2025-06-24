using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSfx : MonoBehaviour
{
    bool cantSfx;
    bool sfxCount;

    float sfxTerm;
    bool cantTerm;

    void OnCollisionEnter(Collision collision)
    {
        //PlayHitSfx();
    }

    void OnTriggerEnter(Collider other)
    {
        //PlayHitSfx();
    }

    // �ѹ��� �Ҹ����� ��
    void PlayHitSfx()
    {
        if (AudioManager.instance != null && !sfxCount && !cantSfx && !cantTerm)
        {
            cantTerm = true;

            Debug.Log("�΋H���� �Ҹ� ���ߵ� ");
            //StartCoroutine(OffSfx());
            //StartCoroutine(TermSfx());

            switch (VfxManager.instance.stack)
            {
                case 0:
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
                    break;
                case 1:
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox2);
                    break;
                case 2:
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox3);
                    break;
                default:
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
                    break;
            }

            sfxCount = true;
        }
    }

    IEnumerator OffSfx()
    {
        yield return new WaitForSeconds(1f);
        cantSfx = true;
    }

    IEnumerator TermSfx()
    {
        yield return new WaitForSeconds(0.5f);
        cantTerm = false;
    }
}
