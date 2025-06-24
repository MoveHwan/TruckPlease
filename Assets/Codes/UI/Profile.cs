using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public static Profile Instance;

    public Image ProfileImage;
    public TextMeshProUGUI ProfileName;
    public TextMeshProUGUI ProfileTotalStar;
    public TextMeshProUGUI ProfileTotalWeight;

    private void Awake()
    {
        Instance = this;

        ProfileName.text = PlayerPrefs.GetString("nickname", "Guest");

        ProfileTotalWeight.text = PlayerPrefs.GetFloat("TotalWeight", 0).ToString();

        if (Social.localUser.authenticated)
            StartCoroutine(GetPlayerImage());
    }


    IEnumerator GetPlayerImage()
    {
        while (Social.localUser.image == null)
        {
            yield return null;
        }

        ProfileImage.sprite = Sprite.Create(
                Social.localUser.image,
                new Rect(0, 0, Social.localUser.image.width, Social.localUser.image.height),
                new Vector2(0.5f, 0.5f)
            );

        ProfileImage.gameObject.SetActive(true);

        yield break;
    }

    public void SetTotalStar(int totalStar)
    {
        ProfileTotalStar.text = totalStar.ToString();
    }
}
