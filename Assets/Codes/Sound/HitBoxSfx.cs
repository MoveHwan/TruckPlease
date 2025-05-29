using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSfx : MonoBehaviour
{
    bool cantSfx;
    int sfxCount = 0;

    float sfxTerm;
    bool cantTerm;

    void OnCollisionEnter(Collision collision)
    {
        PlayHitSfx();
    }

    void OnTriggerEnter(Collider other)
    {
        PlayHitSfx();
    }

    void PlayHitSfx()
    {
        if (AudioManager.instance != null && sfxCount <= 3 && !cantSfx && !cantTerm)
        {
            cantTerm = true;

            Debug.Log("何婬洒绰 家府 唱具凳 ");
            StartCoroutine(OffSfx());
            StartCoroutine(TermSfx());
            AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox);
            sfxCount++;
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
