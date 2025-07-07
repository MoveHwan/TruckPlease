using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public static VfxManager instance;

    public int stack;   // �޺� ����
    public bool comboPop;   //�޺��� �������� Ȯ��

    public ParticleSystem firstStack;
    public ParticleSystem secondStack;
    public ParticleSystem thirdStack;
    public ParticleSystem star;

    private void Awake()
    {
        instance = this;
    }
}
