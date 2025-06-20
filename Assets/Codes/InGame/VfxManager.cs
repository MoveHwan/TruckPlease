using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public static VfxManager instance;

    public int stack;   // ÄÞº¸ ½ºÅÃ

    private void Awake()
    {
        instance = this;
    }
}
