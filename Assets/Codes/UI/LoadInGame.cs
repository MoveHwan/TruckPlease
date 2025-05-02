using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInGame : MonoBehaviour
{


    public void InGameLoad()
    {
        SceneManager.LoadScene("InGame");
    }

}
