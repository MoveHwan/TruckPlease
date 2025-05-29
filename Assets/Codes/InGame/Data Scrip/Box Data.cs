using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Scriptable Object/Box Data")]
public class BoxData : ScriptableObject
{
    public enum BoxType { basicV, longV, bigV, tallV ,smallV}

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
                case BoxType.smallV:
                    return 0.5f;


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
                case BoxType.smallV:
                    return 0.5f;


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
                    return 15f;
                case BoxType.longV:
                    return 18f;
                case BoxType.bigV:
                    return 22f;
                case BoxType.tallV:
                    return 25f;
                case BoxType.smallV:
                    return 10f;


                default:
                    return 10f;
            }
        }
    }


}
