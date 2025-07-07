using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public static VfxManager instance;

    public int stack;   // 콤보 스택
    public bool comboPop;   //콤보가 터졌는지 확인

    public ParticleSystem firstStack;
    public ParticleSystem secondStack;
    public ParticleSystem thirdStack;
    public ParticleSystem star;

    private void Awake()
    {
        instance = this;
    }
}
