using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCheck : MonoBehaviour
{
    public static StageCheck Instance;

    private void Awake()
    {
        Instance = this;
    }


}
