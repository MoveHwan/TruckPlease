using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInGame : MonoBehaviour
{


    public void LoadStage(int StageNum)
    {
        PlayerPrefs.SetInt("Stage", StageNum);

        SceneManager.LoadScene("InGame");
    }

}
