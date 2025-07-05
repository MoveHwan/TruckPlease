using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImageManager : MonoBehaviour
{
    public static ProfileImageManager Instance;

    public Texture PlayerTexture;

    [Space]
    public GameObject ProfilePrefab;
    public List<Texture> ProfileTextures;

    [Header("[ ProfileImageUI ]")]
    public GameObject ProfileImageUI;
    public RectTransform Content;
    public RawImage ProfileRawImage;
    public RawImage PopupRawImage;
    public RawImage ChangeRawImage;
    public CanvasGroup NoGoldMessage;

    [Header("[ BuyImageUI ]")]
    public GameObject BuyImageCheckUI;
    public RawImage BuyImage;
    public Button BuyButton;


    static List<string> BuyImages;

    [SerializeField] GameObject PrevSelectObj;
    [SerializeField] Texture ChangeTexture;
    Sequence mesSeq;

    void Awake()
    {
        //PlayerPrefs.SetInt("Gold", 1000);

        if (Instance == null) 
            Instance = this;

        BuyImages = new List<string>(PlayerPrefs.GetString("BuyImages", "Human_1").Split(","));

        if (Content.childCount <= 0)
        {
            GameObject profileObj;

            for (int i = 0; i < ProfileTextures.Count; i++)
            {
                profileObj = Instantiate(ProfilePrefab, Content);
                profileObj.name = ProfileTextures[i].name;
                profileObj.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = ProfileTextures[i];

                profileObj.SetActive(true);
            }
        }

        string imageName = PlayerPrefs.GetString("ProfileImage", "Human_1");

        PlayerTexture = ProfileTextures.Find(texture => texture.name == imageName);
        
        ProfileRawImage.texture = PlayerTexture;
        PopupRawImage.texture = PlayerTexture;
        ChangeRawImage.texture = PlayerTexture;

        NoGoldMessage.gameObject.SetActive(false);
    }


    public void ConfirmSelectImage(Transform btn)
    {
        if (ChangeTexture.name == PlayerTexture.name) return;

        if (PlayerPrefs.GetInt("Gold", 0) < 500)
        {
            PlayMessage(btn);
            return;
        }

        PlayerTexture = ChangeTexture;

        ProfileRawImage.texture = PlayerTexture;
        PopupRawImage.texture = PlayerTexture;

        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 500);
        PlayerPrefs.SetString("ProfileImage", PlayerTexture.name);
        PlayerPrefs.Save();

        ProfileImageUI.SetActive(false);
    }

    public void ChangeImageSelect(string name, GameObject SelectObj) 
    {
        if (PrevSelectObj != null)
        {
            if (PrevSelectObj.name == SelectObj.name) return;

            PrevSelectObj.GetComponent<ProfileImage>().SelectOutline.SetActive(false);
        }

        PrevSelectObj = SelectObj;


        ChangeTexture = ProfileTextures.Find(texture => texture.name == name);
        ChangeRawImage.texture = ChangeTexture;
    }

    public void BuyImageUIOn(string name, Transform imgBuyBtn)
    {
        if (PlayerPrefs.GetInt("Gold", 0) < 200)
        {
            PlayMessage(imgBuyBtn.GetChild(0));
            return;
        }


        BuyImage.texture = ProfileTextures.Find(texture => texture.name == name);

        BuyButton.onClick.RemoveAllListeners();
        BuyButton.onClick.AddListener(() => 
        {
            if (PlayerPrefs.GetInt("Gold", 0) < 200) return;

            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 200);
            PlayerPrefs.SetString("BuyImages", PlayerPrefs.GetString("BuyImages", "Human_1") + "," + name);
            PlayerPrefs.Save();

            BuyImages.Add(name);

            BuyImageCheckUI.SetActive(false);
            imgBuyBtn.gameObject.SetActive(false);
        });

        BuyImageCheckUI.SetActive(true);
    }

    public bool CheckIsBuyImage(string name)
    {
        return BuyImages.Contains(name);
    }


    void PlayMessage(Transform target)
    {
        if (mesSeq != null && mesSeq.IsPlaying())
            mesSeq.Kill();
        
        mesSeq = DOTween.Sequence();

        mesSeq
            .AppendCallback(() =>
            {
                NoGoldMessage.gameObject.SetActive(false);

                RectTransform NoGoldRect = NoGoldMessage.GetComponent<RectTransform>();

                NoGoldRect.anchoredPosition = NoGoldRect.parent.InverseTransformPoint(target.position);
                NoGoldRect.anchoredPosition += Vector2.right * 100 + Vector2.up * NoGoldRect.sizeDelta.y;

                NoGoldMessage.alpha = 0;
                NoGoldMessage.transform.localScale = Vector3.one * 0.8f;

                NoGoldMessage.gameObject.SetActive(true);
            })
            .Append(NoGoldMessage.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack))
            .Join(NoGoldMessage.DOFade(1f, 0.3f))
            .AppendInterval(0.3f)
            .Append(NoGoldMessage.DOFade(0, 0.3f))
            .AppendCallback(() => NoGoldMessage.gameObject.SetActive(false));


        Debug.LogWarning("Not enough Gold");
    }

    public void Close()
    {
        ProfileRawImage.texture = PlayerTexture;
        PopupRawImage.texture = PlayerTexture;
        ChangeRawImage.texture = PlayerTexture;
    }
}
