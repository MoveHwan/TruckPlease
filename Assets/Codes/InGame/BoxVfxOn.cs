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
        Debug.Log("�ϴ� �΋H��");
        if (!vfxOn && BoxManager.Instance.GoaledBoxes.Contains(gameObject))
        {
            Debug.Log("ȿ�� ���ߵ�");

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
        switch (VfxManager.instance.stack) 
        { 
            case 0: 
                firstStack.Play();
                VfxManager.instance.stack++;
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox1);
                firstStack.transform.position = gameObject.transform.position;
                break;
            case 1: 
                secondStack.Play();
                VfxManager.instance.stack++;
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox2);
                secondStack.transform.position = gameObject.transform.position;
                break;
            case 2: 
                thirdStack.Play();
                star.Play();
                AudioManager.instance.PlaySfx(AudioManager.Sfx.coinSound);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.hitBox3);
                InGameGoldUI.Instance.GetGold();
                thirdStack.transform.position = gameObject.transform.position;
                star.transform.position = gameObject.transform.position;
                break;
        }
    }
}
