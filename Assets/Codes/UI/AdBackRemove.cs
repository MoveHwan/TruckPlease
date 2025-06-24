using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBackRemove : MonoBehaviour
{
    public static AdBackRemove instance;

    public bool ADEnd;

    void Awake()
    {
        instance = this;
    }

    
    void Update()
    {
        if (ADEnd && gameObject.activeSelf)
        {
            ADEnd = false;
            gameObject.SetActive(false);
        }
            
    }
}
