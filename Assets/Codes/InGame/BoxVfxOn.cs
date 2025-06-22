using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem firstStack;   
    public ParticleSystem secondStack; 
    public ParticleSystem thirdStack;
    public ParticleSystem star;

    bool vfxOn;

    void Awake()
    {
        firstStack = VfxManager.instance.firstStack;
        secondStack = VfxManager.instance.secondStack;
        thirdStack = VfxManager.instance.thirdStack;
        star = VfxManager.instance.star;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ÀÏ´Ü ºÎ‹HÈû");
        if (!vfxOn && BoxManager.Instance.GoaledBoxes.Contains(gameObject))
        {
            Debug.Log("È¿°ú ³ª¾ßµÊ");

            ParticlePlay();
            vfxOn = true;
        }
    }

    public void ParticlePlay()
    {
        switch (VfxManager.instance.stack) 
        { 
            case 0: 
                firstStack.Play();
                VfxManager.instance.stack++;

                firstStack.transform.position = gameObject.transform.position;
                break;
            case 1: 
                secondStack.Play();
                VfxManager.instance.stack++;

                secondStack.transform.position = gameObject.transform.position;
                break;
            case 2: 
                thirdStack.Play();
                star.Play();
                thirdStack.transform.position = gameObject.transform.position;
                star.transform.position = gameObject.transform.position;
                break;
        }
    }
}
