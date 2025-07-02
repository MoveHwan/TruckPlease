using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Authentication;
using Newtonsoft.Json;
using DG.Tweening;
using Unity.VisualScripting;

public class Profile : MonoBehaviour
{
    public static Profile Instance;

    public TextAsset SlangFile;

    [Header("Profile")]
    public TextMeshProUGUI ProfileName;
    public TextMeshProUGUI ProfileTotalStar;
    public TextMeshProUGUI ProfileTotalWeight;
    public GameObject FreeMessage;

    [Header("Profile Popup")]
    public TMP_InputField PopupInput;
    public TextMeshProUGUI PopupPID;
    public TextMeshProUGUI PopupName;
    public TextMeshProUGUI PopupTotalStar;
    public TextMeshProUGUI PopupTotalWeight;
    public TextMeshProUGUI PopupTotalStage;
    public TextMeshProUGUI Mesaage;
    public TextMeshProUGUI NickPrice;
    public GameObject CheckButton;
    public GameObject ModifyButton;

    static HashSet<string> SlangList;
    [SerializeField] string changeName, slangStr;


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

        if (PlayerPrefs.GetInt("FreeNick", 0) == 0)
        {
            FreeMessage.SetActive(true);
            NickPrice.text = "Free";
        }
        else
        {
            FreeMessage.SetActive(false);
            NickPrice.text = "200";
        }

        Cancel(true);
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

        CheckButton.SetActive(true);

        PopupName.text = "Input User Name";
        PopupInput.text = "";
    }
    public void Cancel(bool isNick)
    {
        Mesaage.transform.parent.gameObject.SetActive(false);

        CheckButton.SetActive(true);

        PopupName.text = PlayerPrefs.GetString("nickname", "User name");
        PopupInput.text = "";
    }

    public void OnValueChanged(string text)
    {
        if (changeName == text) return;

        Mesaage.transform.parent.gameObject.SetActive(false);

        changeName = text;
    }

    public void ConfirmNick()
    {
        if (PlayerPrefs.GetInt("FreeNick", 0) == 0)
        {
            PlayerPrefs.SetInt("FreeNick", 1);

            PlayerPrefs.SetString("nickname", changeName);

            FreeMessage.SetActive(false);

            NickPrice.text = "200";
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

        PlayerPrefs.Save();

        if (GameDatas.instance != null)
            GameDatas.instance.CloudSave();

        ProfileName.text = changeName;
        PopupName.text = changeName;

        Cancel(true);

        ModifyButton.SetActive(true);
    }

    public void CheckNickName()
    {
        if (Check(changeName)) return;

        MessageOn("Possible User Name!\nUse this name? [ <color #00681B>"+ changeName + "</color> ]");

        CheckButton.SetActive(false);
    }


    bool Check(string name)
    {
        Debug.Log("name: " + name);

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
