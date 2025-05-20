using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVFXCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµðÄ§");

        if (other.CompareTag("Box"))
        {
            Debug.Log(other.name);
            other.GetComponent<BoxVfxOn>().ParticlePlay();
        }
    }
}
