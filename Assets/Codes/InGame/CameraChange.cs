using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject topView;
    public GameObject originStor;
    public GameObject topStor;
    public GameObject Outline;

    bool topViewOn;


    void Update()
    {
        if (topViewOn && BoxManager.Instance.gameEndBox)
        {
            TopviewChange();
        }
    }

    public void TopviewChange()
    {
        if (!topViewOn)
        {
            topViewOn = true;
            topView.SetActive(true);
            originStor.SetActive(false);
            BoxManager.Instance.stopTouch = true;
            topStor.SetActive(true);
            Outline.SetActive(true);

            BottomPanel.instance.HideUI();
        }
        else if (topViewOn) 
        {
            topViewOn = false;
            topView.SetActive(false);
            originStor.SetActive(true);
            topStor.SetActive(false);
            Outline.SetActive(false);
            BoxManager.Instance.stopTouch = false;

            BottomPanel.instance.ShowUI();
        }
    }
}
