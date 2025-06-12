using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBackRemove : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) 
            gameObject.SetActive(false);
    }
}
