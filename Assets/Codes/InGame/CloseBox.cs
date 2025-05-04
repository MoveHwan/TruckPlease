using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBox : MonoBehaviour
{
    public void DeleteBox()
    {
        BoxManager.Instance.gameObject.SetActive(false);
        Debug.Log("»óÀÚ¾ø¾Ú");
    }
}
