using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour
{
    public GameObject SelectOutline;
    public GameObject ButBtn;

    ProfileImageManager ProfileImageManager;

    void OnEnable()
    {
        if (ProfileImageManager == null)
            ProfileImageManager = ProfileImageManager.Instance;

        if (name == PlayerPrefs.GetString("ProfileImage", "Human_1"))
        {
            SelectOutline.SetActive(true);
            ProfileImageManager.ChangeImageSelect(name, gameObject);
        }
        else
            SelectOutline.SetActive(false);

        ButBtn.SetActive(!ProfileImageManager.CheckIsBuyImage(name));   
    }

    public void SelectImage()
    {
        SelectOutline.SetActive(true);
        ProfileImageManager.ChangeImageSelect(name, gameObject);
    }

    public void BuyCheck()
    {
        ProfileImageManager.BuyImageUIOn(name, ButBtn.transform);
    }
}
