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
                    return 1f;
                case BoxType.longV:
                    return 2.0f;
                case BoxType.bigV:
                    return 3f;
                default:
                    return 1.0f;
            }
        }
    }

    public float rbWeight
    {
        get
        {
            switch (boxType)
            {
                case BoxType.basicV:
                    return 1f;
                case BoxType.longV:
                    return 1f;
                case BoxType.bigV:
                    return 2f;
                default:
                    return 1.0f;
            }
        }
    }

    public float forceMultiplier
    {
        get
        {
            switch (boxType)
            {
                case BoxType.basicV:
                    return 12f;
                case BoxType.longV:
                    return 12f;
                case BoxType.bigV:
                    return 18f;
                default:
                    return 10f;
            }
        }
    }


}
