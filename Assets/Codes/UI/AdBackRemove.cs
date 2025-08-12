using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBackRemove : MonoBehaviour
{
    public static AdBackRemove instance;

    public GameObject AdBack;

    public bool ADEnd;

    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        if (ADEnd && AdBack.activeSelf)
        {
            ADEnd = false;
            AdBack.SetActive(false);
        }
            
    }

    public void AdBackOn()
    {
        ADEnd = false;
        AdBack.SetActive(true);
    }
}
