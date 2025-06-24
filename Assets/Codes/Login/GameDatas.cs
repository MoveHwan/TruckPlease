using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DataSettings
{
    public string ID = "None";

    public int Gold = 0;
    public int Fatigue = 0;

    public int Item_Save = 3;
    public int Item_Delete = 3;
    
    public bool Tutorial = false;
    public bool RemoveAd = false;
    public bool IsReview = false;

    public string nickname = "";
    public string TopRatingStage = "1_0";
    public string LastFatigueTime = DateTime.Now.ToString();

    public float BgmVol = 0.5f;
    public float SfxVol = 0.5f;
    public float TotalWeight = 0;

    public List<int> StageStar = new List<int>();
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas instance;

    public DataSettings dataSettings = new DataSettings();
    private string fileName = "Player_Save_Test";


    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        StartCoroutine(WaitGoogleLogin());
    }

    // 구글 게임즈 로그인 대기
    IEnumerator WaitGoogleLogin()
    {
        while (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            if (SceneManager.GetActiveScene().name != "DataLoad")
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }

        PlayGamesPlatform.DebugLogEnabled = true;


        dataSettings.ID = PlayerPrefs.GetString("PlayerID", "None");

        // 아이디가 등록이 안되어잇을경우
        if (dataSettings.ID == "None")
        {
            dataSettings.ID = Social.localUser.id;

            PlayerPrefs.SetString("PlayerID", dataSettings.ID);


            Debug.LogWarning("No PlayerPrefs ID");
            Debug.Log($"Set PlayerID: {dataSettings.ID}");
        }
        // 아이디가 다를경우
        else if (dataSettings.ID != Social.localUser.id)
        {
            ResetPlayerPrefs();

            PlayerPrefs.SetInt("SetPlayerData", 0);

            Debug.LogWarning("아이디가 다름");
        }

        

        if (PlayerPrefs.GetInt("SetPlayerData", 0) == 0)
        {
            LoadData();
            PlayerPrefs.SetInt("SetPlayerData", 1);
        }
        else
        {
            PlayerPrefs_To_DataSettings();
            PlayerPrefsStageStarSet();
        }


        yield break;
    }

 
    // 플레이어프렙 현재 데이터 클라우드에 저장
    public void CloudSave()
    {
        PlayerPrefs_To_DataSettings();

        SaveData();
    }


    // 클라우드 데이터가 비었을 경우 플레이어프렙 데이터 dataSettings에 저장
    void NoPlayerData_Save()
    {
        PlayerPrefs_To_DataSettings();

        PlayerPrefsStageStarSet();

        SaveData();
    }
    // 클라우드 데이터가 있을 경우 플레이어프렙 데이터 dataSettings에 저장
    void LoadCloudSetData()
    {
        DataSettings_To_PlayerPrefs();

        SetStageStar();
    }

    

    //  dataSettings 현재 데이터 플레이어프렙에 저장
    void DataSettings_To_PlayerPrefs()
    {
        PlayerPrefs.SetString("PlayerID", dataSettings.ID);

        PlayerPrefs.SetInt("Gold", dataSettings.Gold);
        PlayerPrefs.SetInt("Fatigue", dataSettings.Fatigue);

        PlayerPrefs.SetInt("Item_Save", dataSettings.Item_Save);
        PlayerPrefs.SetInt("Item_Delete", dataSettings.Item_Delete);

        PlayerPrefs.SetInt("Tutorial", dataSettings.Tutorial ? 1 : 0);
        PlayerPrefs.SetInt("RemoveAd", dataSettings.RemoveAd ? 1 : 0);
        PlayerPrefs.SetInt("ReviewOn", dataSettings.IsReview ? 1 : 0);

        PlayerPrefs.SetString("nickname", dataSettings.nickname);
        PlayerPrefs.SetString("TopRatingStage", dataSettings.TopRatingStage);
        PlayerPrefs.SetString("LastFatigueTime", dataSettings.LastFatigueTime == "" || dataSettings.LastFatigueTime == null ? DateTime.Now.AddDays(-1).ToString() : dataSettings.LastFatigueTime);

        PlayerPrefs.SetFloat("BgmVol", dataSettings.BgmVol);
        PlayerPrefs.SetFloat("SfxVol", dataSettings.SfxVol);
        PlayerPrefs.SetFloat("TotalWeight", dataSettings.TotalWeight);

        PlayerPrefs.Save();
    }

    // 플레이어프렙 현재 데이터 dataSettings에 저장
    void PlayerPrefs_To_DataSettings()
    {
        dataSettings.Gold = PlayerPrefs.GetInt("Gold", 0);
        dataSettings.Fatigue = PlayerPrefs.GetInt("Fatigue", 10);

        dataSettings.Item_Save = PlayerPrefs.GetInt("Item_Save", 3);
        dataSettings.Item_Delete = PlayerPrefs.GetInt("Item_Delete", 3);

        dataSettings.Tutorial = PlayerPrefs.GetInt("Tutorial", 0) == 1;
        dataSettings.RemoveAd = PlayerPrefs.GetInt("RemoveAd", 0) == 1;
        dataSettings.IsReview = PlayerPrefs.GetInt("ReviewOn", 0) == 1;

        dataSettings.nickname = PlayerPrefs.GetString("nickname");
        dataSettings.TopRatingStage = PlayerPrefs.GetString("TopRatingStage", "1_0");
        dataSettings.LastFatigueTime = PlayerPrefs.GetString("LastFatigueTime", DateTime.Now.ToString());

        dataSettings.BgmVol = PlayerPrefs.GetFloat("BgmVol", 0.5f);
        dataSettings.SfxVol = PlayerPrefs.GetFloat("SfxVol", 0.5f);
        dataSettings.TotalWeight = PlayerPrefs.GetFloat("TotalWeight", 0);
    }

    // 플레이어프렙 리셋
    void ResetPlayerPrefs()
    {
        ResetStageStar();

        PlayerPrefs.SetString("PlayerID", Social.localUser.id);

        PlayerPrefs.SetInt("Gold", 0);
        PlayerPrefs.SetInt("Fatigue", 10);

        PlayerPrefs.SetInt("Item_Save", 3);
        PlayerPrefs.SetInt("Item_Delete", 3);

        PlayerPrefs.SetInt("Tutorial", 0);
        PlayerPrefs.SetInt("RemoveAd", 0);
        PlayerPrefs.SetInt("ReviewOn", 0);
        PlayerPrefs.SetInt("StageIn", 0);
        PlayerPrefs.SetInt("Chapter_Idx", 0);

        int randomNum = Random.Range(0, 99999);
        PlayerPrefs.SetString("nickname", "player" + randomNum.ToString());

        PlayerPrefs.SetString("TopRatingStage", "1_0");
        PlayerPrefs.SetString("LastFatigueTime", DateTime.Now.ToString());

        PlayerPrefs.SetFloat("BgmVol", 0.5f);
        PlayerPrefs.SetFloat("SfxVol", 0.5f);
        PlayerPrefs.SetFloat("TotalWeight", 0);

        PlayerPrefs.Save();
    }



    // dataSettings 스테이지별 플레이어프렙에 저장
    void SetStageStar()
    {
        for (int i = 0; i < dataSettings.StageStar.Count; i++)
        {
            PlayerPrefs.SetInt("Stage" + (i + 1) + "_Star", dataSettings.StageStar[i]);
        }

    }

    // 플레이어프렙 현재 스테이지 별 dataSettings에 추가
    public void NewStageStar()
    {
        int stage = PlayerPrefs.GetInt("Stage", 0);
        int starCount = PlayerPrefs.GetInt("Stage" + stage + "_Star", 0);

        if (starCount <= 0 || stage <= 0) return;

        // 리스트가 targetIndex보다 작으면 0으로 채우기
        while (dataSettings.StageStar.Count <= stage-1)
        {
            dataSettings.StageStar.Add(0);
        }

        if (dataSettings.StageStar[stage - 1] < starCount)
            dataSettings.StageStar[stage - 1] = starCount;
    }

    // 플레이어프렙 스테이지 별 dataSettings에 저장
    void PlayerPrefsStageStarSet()
    {
        int topChap = int.Parse(dataSettings.TopRatingStage.Split("_")[0]);
        int topStage = int.Parse(dataSettings.TopRatingStage.Split("_")[1]);

        int starStage = 0;

        for (int chap = 1; chap <= topChap; chap++)
        {
            for (int stage = 1; stage <= 12; stage++)
            {
                if (chap >= topChap && stage > topStage) return;
                if (chap == 1 && stage > 9) break;

                Debug.Log($"chap: {chap}, stage: {stage}, star: {PlayerPrefs.GetInt("Stage" + ++starStage + "_Star", 0)}");

                dataSettings.StageStar.Add(PlayerPrefs.GetInt("Stage" + starStage + "_Star", 0));
            }
        }

    }

    // 플레이어프렙 스테이지 별 리셋
    void ResetStageStar()
    {
        int chap = int.Parse(PlayerPrefs.GetString("TopRatingStage", "1_0").Split("_")[0]);

        for (int i = 0; i < chap; i++)
        {
            for (int stage = 0; stage < 12; stage++)
            {
                if (i == 0 && stage > 9) break;

                PlayerPrefs.SetInt("Stage" + (stage + 1) + "_Star", 0);
            }

        }

    }

    

   
    void SaveData()
    {
        OpenSaveGame();
    }

    private void OpenSaveGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood , OnsavedGameOpened);
    }

    private void OnsavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("저장 성공");

            var update = new SavedGameMetadataUpdate.Builder().Build();

            //JSON
            var json = JsonUtility.ToJson(dataSettings);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            string data = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("저장 데이터: " + data);

            savedGameClient.CommitUpdate(game, update, bytes, OnsavedGameWritten);
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }

    private void OnsavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("저장 성공");
        }
        else
        {
            Debug.Log("저장 실패");
        }
    }

    void LoadData()
    {
        OpenLoadGame();
    }

    private void OpenLoadGame()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGameData);
    }

    void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        Debug.Log("LoadGameData status: " + status);

        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("로드 성공");

            savedGameClient.ReadBinaryData(data, OnSavedGameDataRead);
        }
        else
        {
            PlayerPrefs.SetInt("SetPlayerData", 0);
            Debug.Log("로드 실패");
        }
    }

    void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if(data == "")
        {
            Debug.Log("데이터 없음 초기 데이터 저장");

            NoPlayerData_Save();
        }
        else
        {
            Debug.Log("로드 데이터 : " + data);

            dataSettings = JsonUtility.FromJson<DataSettings>(data);

            DataSettings defaultData = new DataSettings();

            LoadCloudSetData();
        }
    }

    public void DeleteData()
    {
        DeleteGameData();
    }

    void DeleteGameData()
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        savedGameClient.OpenWithAutomaticConflictResolution(fileName, GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, DeleteSaveGame);
    }

    private void DeleteSaveGame(SavedGameRequestStatus status, ISavedGameMetadata data)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        if(status == SavedGameRequestStatus.Success)
        {
            savedGameClient.Delete(data);
            Debug.Log("삭제 성공");
        }
        else
        {
            Debug.Log("삭제 실패");
        }
    }
}
