using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackIntAd : MonoBehaviour
{
    public static StackIntAd instance;

    public int stack;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
