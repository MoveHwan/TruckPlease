using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Scriptable Object/Box Data")]
public class BoxData : ScriptableObject
{
    public enum BoxType { basicV, longV, bigV, tallV }

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
                case BoxType.tallV:
                    return 4f;

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
                    return 1.5f;
                case BoxType.bigV:
                    return 2f;
                case BoxType.tallV:
                    return 2.5f;

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
                    return 15f;
                case BoxType.bigV:
                    return 18f;
                case BoxType.tallV:
                    return 20f;

                default:
                    return 10f;
            }
        }
    }


}
