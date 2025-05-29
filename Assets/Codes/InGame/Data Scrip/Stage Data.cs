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
    public List<GameObject> boxes;

    public float firstStar;
    public float secondStar;
    public float thirdStar;

    [Header("Wind Settings")]
    public bool useWind = false;  // 바람 설정 사용할지 여부
    public bool random = false;     // 랜덤 바람일지
    public WindManager.WindType windType;
    public WindManager.WindSpeed windSpeed;
}
