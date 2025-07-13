using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBoxDel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ºÎµğÄ§");

        if (other.CompareTag("Box"))
        {
            StartCoroutine(BoxDelayDelete(other.gameObject));
            BoxManager.Instance.CalcBoxCurEnd();
            VfxManager.instance.stack = 0;
            if(GameManager.Instance.life > 0)
            {
                GameManager.Instance.life--;
                if(GameManager.Instance.life <= 0)
                {
                    StartCoroutine(BoxManager.Instance.BoxGameEnd());
                }
            }

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
