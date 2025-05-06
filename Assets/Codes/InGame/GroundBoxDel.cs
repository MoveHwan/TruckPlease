using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoxDel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµðÄ§");

        if (other.CompareTag("Box"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
