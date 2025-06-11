using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaManager : MonoBehaviour
{
    public GameObject RemoveADBtn;
    public GameObject RemoveADPopUp;

    GameDatas GameDatas;


    // Start is called before the first frame update
    void Start()
    {
        GameDatas = GameDatas.instance;

        RemoveADPopUp.SetActive(false);

        if (PlayerPrefs.GetInt("RemoveAd",0) == 1)
        {
            RemoveADBtn.SetActive(false);
        }
        else
        {
            RemoveADBtn.SetActive(true);
        }
    }

    public void PurchaComplete_RemoveAD()
    {
        PlayerPrefs.SetInt("RemoveAd", 1);

        GameDatas.PlayerSetData();

        RemoveADPopUp.SetActive(false);
        RemoveADBtn.SetActive(false);
    }
}
