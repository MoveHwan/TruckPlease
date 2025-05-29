using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµðÄ§");

        if (other.CompareTag("Box") && !BoxManager.Instance.GoaledBoxes.Contains(other.gameObject))
        {
            BoxManager.Instance.GoaledBoxes.Add(other.gameObject);
            ThrowBox throwBox = other.GetComponent<ThrowBox>();
            throwBox.canWind = false;
            Debug.Log($"Added: {other.name}");
            BoxManager.Instance.CalcBoxIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") && BoxManager.Instance.GoaledBoxes.Contains(other.gameObject))
        {
            BoxManager.Instance.GoaledBoxes.Remove(other.gameObject);
            Debug.Log($"Removed: {other.name}");
            BoxManager.Instance.CalcBoxIn();
        }
    }

}
