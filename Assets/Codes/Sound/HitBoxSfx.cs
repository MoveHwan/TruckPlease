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
        PlayHitSfx();
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
            AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox);
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
