using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem firstStack;   
    public ParticleSystem secondStack; 
    public ParticleSystem thirdStack;

    bool vfxOn;
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ÀÏ´Ü ºÎ‹HÈû");
        if (!vfxOn && BoxManager.Instance.GoaledBoxes.Contains(gameObject))
        {
            Debug.Log("È¿°ú ³ª¾ßµÊ");

            ParticlePlay();
            VfxManager.instance.stack++;
            vfxOn = true;
        }
    }

    public void ParticlePlay()
    {
        switch (VfxManager.instance.stack) 
        { 
            case 0: firstStack.Play();break;
            case 1: secondStack.Play(); break;
            case 2: thirdStack.Play();break;
        }
    }
}
