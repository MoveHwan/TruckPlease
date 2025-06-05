using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DataSettings
{
    public int gold = 0;
    public float bestTime = 0;
    public bool isCheck = false;
    public float[] skillTime = { 3, 6, 9 };
}

public class GameDatas : MonoBehaviour
{
    public DataSettings dataSettings = new DataSettings();
    private string fileName = "save.dat";

    public void SaveData()
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

    public void LoadData()
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

        if(status == SavedGameRequestStatus.Success)
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
            SaveData();
        }
        else
        {
            Debug.Log("로드 데이터 : " + data);

            dataSettings = JsonUtility.FromJson<DataSettings>(data);
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
