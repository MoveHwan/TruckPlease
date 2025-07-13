using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Box", menuName = "Scriptable Object/Box Data")]
public class BoxData : ScriptableObject
{
    public enum BoxType { Red, Orange, Yellow, Green, Blue, Pink, Skyblue, Purple}

    public BoxType boxType;

    public float Weight
    {
        get
        {
            switch (boxType)
            {
                case BoxType.Red:
                    return 1f;
                case BoxType.Orange:
                    return 3f;
                case BoxType.Yellow:
                    return 10f;
                case BoxType.Green:
                    return 30f;
                case BoxType.Blue:
                    return 80f;
                case BoxType.Pink:
                    return 200f;
                case BoxType.Skyblue:
                    return 500f;
                case BoxType.Purple:
                    return 1500f;

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
                //case BoxType.basicV:
                //    return 0.1f;
                //case BoxType.longV:
                //    return 1.5f;
                //case BoxType.bigV:
                //    return 2f;
                //case BoxType.tallV:
                //    return 2.5f;
                //case BoxType.smallV:
                //    return 0.5f;


                default:
                    return 0.1f;
            }
        }
    }

    public float forceMultiplier
    {
        get
        {
            switch (boxType)
            {
                //case BoxType.basicV:
                //    return 1.3f;
                //case BoxType.longV:
                //    return 18f;
                //case BoxType.bigV:
                //    return 22f;
                //case BoxType.tallV:
                //    return 25f;
                //case BoxType.smallV:
                //    return 10f;


                default:
                    return 1.3f;
            }
        }
    }


}
