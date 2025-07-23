using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBox : MonoBehaviour
{
    public static LifeBox instance;

    public GameObject[] BoxLlfes;

    int boxIdx;

    void Awake()
    {
        instance = this;
    }

    public void SubLife()
    {
        if (boxIdx > BoxLlfes.Length - 1) return;

        BoxLlfes[boxIdx++].SetActive(true);
    }
}
