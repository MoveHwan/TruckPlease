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
            StartCoroutine(BoxDelayDelete(other.gameObject));
            BoxManager.Instance.CalcBoxCurEnd();
        }
    }


    IEnumerator BoxDelayDelete(GameObject box)
    {
        yield return new WaitForSeconds(1f);
        if (box != null) 
        { 
            box.SetActive(false);
        }
    }
}
