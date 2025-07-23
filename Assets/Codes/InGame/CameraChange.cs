using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject topView;
    public GameObject originStor;
    public GameObject topStor;

    bool topViewOn;

    public void TopviewChange()
    {
        if (!topViewOn)
        {
            topViewOn = true;
            topView.SetActive(true);
            originStor.SetActive(false);
            BoxManager.Instance.stopTouch = true;
            topStor.SetActive(true);
        }
        else if (topViewOn) 
        {
            topViewOn = false;
            topView.SetActive(false);
            originStor.SetActive(true);
            topStor.SetActive(false);
            BoxManager.Instance.stopTouch = false;

        }
    }
}
