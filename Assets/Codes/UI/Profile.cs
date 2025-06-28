using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Newtonsoft.Json;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Profile : MonoBehaviour
{
    public static Profile Instance;

    public TextAsset SlangFile;

    [Header("Profile")]
    public RawImage ProfileImage;
    public TextMeshProUGUI ProfileName;
    public TextMeshProUGUI ProfileTotalStar;
    public TextMeshProUGUI ProfileTotalWeight;
    public GameObject FreeMessage;

    [Header("Profile Popup")]
    public RawImage PopupImage;
    public TMP_InputField PopupInput;
    public TextMeshProUGUI PopupPID;
    public TextMeshProUGUI PopupName;
    public TextMeshProUGUI PopupTotalStar;
    public TextMeshProUGUI PopupTotalWeight;
    public TextMeshProUGUI PopupTotalStage;
    public TextMeshProUGUI Mesaage;
    public RectTransform Buttons; 

       
    static HashSet<string> SlangList;
    [SerializeField] string changeName, slangStr;

    Sequence seq;

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (SlangList == null)
            LoadSlangList();

        ProfileName.text = PlayerPrefs.GetString("nickname", "Guest");
        PopupName.text = PlayerPrefs.GetString("nickname", "User name");

        ProfileTotalWeight.text = PlayerPrefs.GetFloat("TotalWeight", 0).ToString();
        PopupTotalWeight.text = PlayerPrefs.GetFloat("TotalWeight", 0).ToString();

        if (Social.localUser.authenticated)
        {
            PopupPID.text = AuthenticationService.Instance.PlayerId;
        }
        else
        {
            PopupPID.text = "None";
            PopupTotalStage.text = " - ";
        }

        FreeMessage.SetActive(PlayerPrefs.GetInt("FreeNick", 0) == 0);

        Cancel();

    }

    public void SetTotalStat(int totalStar, int totalStage)
    {
        ProfileTotalStar.text = totalStar.ToString();
        PopupTotalStar.text = totalStar.ToString();

        PopupTotalStage.text = totalStage.ToString();
    }

    public void Cancel()
    {
        Mesaage.transform.parent.gameObject.SetActive(false);
        Buttons.gameObject.SetActive(false);

        PopupName.text = PlayerPrefs.GetString("nickname", "User name");
        PopupInput.text = "";
    }

    public void OnValueChanged(string text)
    {
        if (changeName == text) return;

        Mesaage.transform.parent.gameObject.SetActive(false);
        Buttons.gameObject.SetActive(false);

        changeName = text;
    }

    public void ConfirmNick()
    {
        if (PlayerPrefs.GetInt("FreeNick", 0) == 0)
        {
            PlayerPrefs.SetInt("FreeNick", 1);

            PlayerPrefs.SetString("nickname", changeName);

            FreeMessage.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("Gold", 0) >= 200)
            {
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 0) - 200);
                PlayerPrefs.SetString("nickname", changeName);
            }
            else
            {
                MessageOn("<color #EA3030>Not enough gold!!</color>");
                return;
            }
        }

        ProfileName.text = changeName;
        PopupName.text = changeName;

        PlayerPrefs.Save();

        if (GameDatas.instance != null)
            GameDatas.instance.CloudSave();

        Cancel();
    }

    public void CheckNickName()
    {
        if (Check(changeName)) return;

        MessageOn("Possible User Name!\nUse this name? [ <color #00681B>"+ changeName + "</color> ]");

        Buttons.gameObject.SetActive(false);

        if (seq == null)
            seq = DOTween.Sequence();
        else if (seq.IsPlaying())
            seq.Kill();

        TextMeshProUGUI PriceText = Buttons.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.GetInt("FreeNick", 0) == 0)
            PriceText.text = "Free";
        else
            PriceText.text = "200";


        // 원래 위치 저장
        Vector2 originalPos = Buttons.anchoredPosition;

        // 캔버스 그룹 없으면 추가
        CanvasGroup canvasGroup = Buttons.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = Buttons.AddComponent<CanvasGroup>();
        
        Buttons.anchoredPosition = originalPos + Vector2.down * Buttons.anchoredPosition;
        canvasGroup.alpha = 0;

        Buttons.gameObject.SetActive(true);

        // 이동 + 알파 동시에 트윈
        seq.Append(Buttons.DOAnchorPos(originalPos, 0.3f).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1f, 0.7f));

    }


    bool Check(string name)
    {
        Debug.Log("name: " + name);

       /* if (!purchaseTicket)
        {
            if (TicketIdx == -1)
            {
                Debug.LogError("닉네임 변경권이 없습니다!");
                ActiveObj("Product");
                buttonBlock = false;
                return true;
            }
        }*/

        if (name == null || name.Length == 0 || name == "")
        {
            MessageOn("Enter User Name!");
            return true;
        }

        if (name == PlayerPrefs.GetString("nickname"))
        {
            MessageOn("Same User Name!");
            return true;
        }

        if (name.Length < 3 || name.Length > 10)
        {
            MessageOn("User Name ( <color #EA3030>3–10 chars</color> )");
            return true;
        }

        if (IsValidNickname(name))
        {
            MessageOn("Contains space or special characters [ <color #EA3030>" + slangStr + "</color> ]");
            return true;
        }

        if (IsValidNickname(name, true))
        {
            MessageOn("Restricted or Banned word.\n[ <color #EA3030>" + slangStr + "</color> ]");
            return true;
        }

        return false;
    }

    // 공백 특수문자 검증
    bool IsValidNickname(string nickname)
    {
        foreach (char ch in nickname)
        {
            int asciiValue = (int)ch;

            // 공백 및 특수문자 (아스키코드)
            /*if (asciiValue == 9 || asciiValue == 10 || (asciiValue > 31 && asciiValue < 48)
                || (asciiValue > 122 && asciiValue < 127) || (asciiValue > 57 && asciiValue < 65) || (asciiValue > 90 && asciiValue < 97))*/
            if (asciiValue < 48 || (asciiValue > 122 && asciiValue < 127) || (asciiValue > 57 && asciiValue < 65)
                || (asciiValue > 90 && asciiValue < 97) || (asciiValue > 167 && asciiValue < 256))
            {
                Debug.LogError(asciiValue);
                slangStr = ch + "";
                return true;
            }
        }

        return false; // 공백과 특수문자 없음
    }
    // 비속어 검증
    bool IsValidNickname(string nickname, bool slag)
    {

        foreach (string slang in SlangList)
        {
            if (nickname.Contains(slang))
            {
                slangStr = slang;
                return true; // 비속어 포함
            }
        }
        return false; // 비속어 없음
    }

    void MessageOn(string message)
    {
        Mesaage.text = message;

        CanvasGroup MesaageCanvas = Mesaage.transform.parent.GetComponent<CanvasGroup>();

        if (MesaageCanvas == null ) 
            MesaageCanvas = Mesaage.transform.parent.AddComponent<CanvasGroup>();

        MesaageCanvas.gameObject.SetActive(false);

        MesaageCanvas.DOKill();

        MesaageCanvas.alpha = 0;
        MesaageCanvas.transform.localScale = Vector3.one * 0.8f;

        MesaageCanvas.gameObject.SetActive(true);

        MesaageCanvas.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        MesaageCanvas.DOFade(1f, 0.3f);

        Debug.LogWarning(message);
    }

    void LoadSlangList()
    {
        // JSON 파일 읽기 및 HashSet으로 역직렬화
        string json = SlangFile.text;
        SlangList = JsonConvert.DeserializeObject<HashSet<string>>(json);
    }

}
