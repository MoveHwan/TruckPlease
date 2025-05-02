using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Scriptable Object/Box Data")]
public class BoxData : ScriptableObject
{
    public enum BoxType { basicV,longV, bigV}

    public BoxType boxType;

    public float Weight
    {
        get
        {
            switch (boxType)
            {
                case BoxType.basicV:
                    return 1.5f;
                case BoxType.longV:
                    return 2.0f;
                case BoxType.bigV:
                    return 3.5f;
                default:
                    return 1.0f;
            }
        }
    }
}
