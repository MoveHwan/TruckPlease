using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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
    public string LastFatigueTime = "";

    public float BgmVol = 0.5f;
    public float SfxVol = 0.5f;
    public float TotalWeight = 0;

    public List<int> StageStar = new List<int>();
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas instance;

    public DataSettings dataSettings = new DataSettings();
    private string fileName = "Player_Save";


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

        PlayGamesPlatform.DebugLogEnabled = true;

        instance = this;

        if (PlayerPrefs.GetInt("SetPlayerData", 0) == 0)
        {
            LoadData();
            PlayerPrefs.SetInt("SetPlayerData", 1);
        }
        
    }

    void LoadCloudSetData()
    {
        PlayerPrefs.SetString("PlayerID", dataSettings.ID);

        PlayerPrefs.SetInt("Gold", dataSettings.Gold);
        PlayerPrefs.SetInt("Fatigue", dataSettings.Fatigue);

        PlayerPrefs.SetInt("Item_Save", dataSettings.Item_Save);
        PlayerPrefs.SetInt("Item_Delete", dataSettings.Item_Delete);

        PlayerPrefs.SetInt("Tutorial", dataSettings.Tutorial ? 1:0);
        PlayerPrefs.SetInt("RemoveAd", dataSettings.RemoveAd ? 1:0);
        PlayerPrefs.SetInt("ReviewOn", dataSettings.IsReview ? 1:0);

        PlayerPrefs.SetString("nickname", dataSettings.nickname);
        PlayerPrefs.SetString("TopRatingStage", dataSettings.TopRatingStage);
        PlayerPrefs.SetString("LastFatigueTime", dataSettings.LastFatigueTime == "" || dataSettings.LastFatigueTime == null ? DateTime.Now.AddDays(-1).ToString() : dataSettings.LastFatigueTime);

        PlayerPrefs.SetFloat("BgmVol", dataSettings.BgmVol);
        PlayerPrefs.SetFloat("SfxVol", dataSettings.SfxVol);
        PlayerPrefs.SetFloat("TotalWeight", dataSettings.TotalWeight);

        SetStageStar();
    }

    void SetStageStar()
    {
        for (int i = 0; i < dataSettings.StageStar.Count; i++) 
        {
            PlayerPrefs.SetInt("Stage" + i + 1 + "_Star", dataSettings.StageStar[i]);
        }

    }

    public void StageEndSave()
    {
        dataSettings.Gold = PlayerPrefs.GetInt("Gold", 0);
        dataSettings.Fatigue = PlayerPrefs.GetInt("Fatigue", 10);

        dataSettings.Item_Save = PlayerPrefs.GetInt("Item_Save", 3);
        dataSettings.Item_Delete = PlayerPrefs.GetInt("Item_Delete", 3);

        dataSettings.Tutorial = PlayerPrefs.GetInt("Tutorial", 0) == 1;
        dataSettings.RemoveAd = PlayerPrefs.GetInt("RemoveAd", 0) == 1;
        dataSettings.IsReview = PlayerPrefs.GetInt("ReviewOn", 0) == 1;

        dataSettings.nickname = PlayerPrefs.GetString("nickname", "");
        dataSettings.TopRatingStage = PlayerPrefs.GetString("TopRatingStage", "1_0");
        dataSettings.LastFatigueTime = PlayerPrefs.GetString("LastFatigueTime", DateTime.Now.ToString());

        dataSettings.BgmVol = PlayerPrefs.GetFloat("BgmVol", 0.5f);
        dataSettings.SfxVol = PlayerPrefs.GetFloat("SfxVol", 0.5f);
        dataSettings.TotalWeight = PlayerPrefs.GetFloat("TotalWeight", 0);

        NewStageStar();

        SaveData();
    }
    void NewStageStar()
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


    public void PlayerDataSave()
    {
        dataSettings.ID = PlayerPrefs.GetString("PlayerID", "None");

        // 아이디가 없는경우
        if (dataSettings.ID == "None")
        {
            Debug.LogWarning("아이디가 등록되어 있지 않음");

            PlayerPrefs.SetString("PlayerID", Social.localUser.id);

            dataSettings.Gold = PlayerPrefs.GetInt("Gold", 0);
            dataSettings.Fatigue = PlayerPrefs.GetInt("Fatigue", 10);

            dataSettings.Item_Save = PlayerPrefs.GetInt("Item_Save", 3);
            dataSettings.Item_Delete = PlayerPrefs.GetInt("Item_Delete", 3);

            dataSettings.Tutorial = PlayerPrefs.GetInt("Tutorial", 0) == 1;
            dataSettings.RemoveAd = PlayerPrefs.GetInt("RemoveAd", 0) == 1;
            dataSettings.IsReview = PlayerPrefs.GetInt("ReviewOn", 0) == 1;

            dataSettings.nickname = PlayerPrefs.GetString("nickname", "");
            dataSettings.TopRatingStage = PlayerPrefs.GetString("TopRatingStage", "1_0");
            dataSettings.LastFatigueTime = PlayerPrefs.GetString("LastFatigueTime", DateTime.Now.ToString());

            dataSettings.BgmVol = PlayerPrefs.GetFloat("BgmVol", 0.5f);
            dataSettings.SfxVol = PlayerPrefs.GetFloat("SfxVol", 0.5f);
            dataSettings.TotalWeight = PlayerPrefs.GetFloat("TotalWeight", 0);

            SaveData();
        }

        // 아이디가 다를경우
        if (dataSettings.ID != Social.localUser.id)
        {
            Debug.LogWarning("아이디가 다름");

            ResetStageStar();

            PlayerPrefs.SetString("PlayerID", Social.localUser.id);

            PlayerPrefs.SetInt("Gold", 0);
            PlayerPrefs.SetInt("Fatigue", 10);

            PlayerPrefs.SetInt("Item_Save", 3);
            PlayerPrefs.SetInt("Item_Delete", 3);

            PlayerPrefs.SetInt("Tutorial", 0);
            PlayerPrefs.SetInt("RemoveAd", 0);
            PlayerPrefs.SetInt("ReviewOn", 0);

            PlayerPrefs.SetString("nickname", "");
            PlayerPrefs.SetString("TopRatingStage", "1_0");
            PlayerPrefs.SetString("LastFatigueTime", DateTime.Now.ToString());

            PlayerPrefs.SetFloat("BgmVol", 0.5f);
            PlayerPrefs.SetFloat("SfxVol", 0.5f);
            PlayerPrefs.SetFloat("TotalWeight", 0);
        }
        else
        {


        }

        dataSettings.Gold = PlayerPrefs.GetInt("Gold", 0);
        dataSettings.Fatigue = PlayerPrefs.GetInt("Fatigue", 10);

        dataSettings.Item_Save = PlayerPrefs.GetInt("Item_Save", 3);
        dataSettings.Item_Delete = PlayerPrefs.GetInt("Item_Delete", 3);

        dataSettings.Tutorial = PlayerPrefs.GetInt("Tutorial", 0) == 1;
        dataSettings.RemoveAd = PlayerPrefs.GetInt("RemoveAd", 0) == 1;
        dataSettings.IsReview = PlayerPrefs.GetInt("ReviewOn", 0) == 1;

        dataSettings.nickname = PlayerPrefs.GetString("nickname", "");
        dataSettings.TopRatingStage = PlayerPrefs.GetString("TopRatingStage", "1_0");
        dataSettings.LastFatigueTime = PlayerPrefs.GetString("LastFatigueTime", DateTime.Now.ToString());

        dataSettings.BgmVol = PlayerPrefs.GetFloat("BgmVol", 0.5f);
        dataSettings.SfxVol = PlayerPrefs.GetFloat("SfxVol", 0.5f);
        dataSettings.TotalWeight = PlayerPrefs.GetFloat("TotalWeight", 0);

        SaveData();
    }

    void PlayerPrefsStageStarSet()
    {
        int chap = int.Parse(dataSettings.TopRatingStage.Split("_")[0]);
        int stage = int.Parse(dataSettings.TopRatingStage.Split("_")[1]);

        for (int c = 0; c < chap; c++)
        {
            for (int s = 0; s < 12; s++)
            {
                if (c + 1 >= chap && s + 1 < stage ) return;
                if (c == 0 && s > 9) break;
            }
        }


        // 리스트가 targetIndex보다 작으면 0으로 채우기
        while (dataSettings.StageStar.Count <= stage - 1)
        {
            dataSettings.StageStar.Add(0);
        }

        /*if (dataSettings.StageStar[stage - 1] < starCount)
            dataSettings.StageStar[stage - 1] = starCount;*/
    }

    void ResetStageStar()
    {
        int chap = int.Parse(PlayerPrefs.GetString("TopRatingStage", "1_0").Split("_")[0]);

        int idx = 0;

        for (int i = 0; i < chap; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                if (i == 0 && j > 9) break;

                PlayerPrefs.SetInt("Stage" + idx + 1 + "_Star", 0);
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

            Debug.Log("저장 데이터: " + bytes);

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
            Debug.Log("로드 실패");
        }
    }

    void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] loadedData)
    {
        string data = System.Text.Encoding.UTF8.GetString(loadedData);

        if(data == "")
        {
            Debug.Log("데이터 없음 초기 데이터 저장");

            PlayerDataSave();
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
