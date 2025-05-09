using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    //public int stage;

    public GameObject truck;
    public GameObject stageWall;
    public GameObject stageCheckBox;
    public GameObject stageObstacle;
    public GameObject[] boxes;

    public float firstStar;
    public float secondStar;
    public float thirdStar;
}
