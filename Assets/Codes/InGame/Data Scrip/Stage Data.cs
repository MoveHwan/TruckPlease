using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stage", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    //public int stage;
    public bool eternal;        // 무한 모드 일 때
    public GameObject truck;
    public GameObject stageWall;
    public GameObject stageCheckBox;
    public GameObject stageObstacle;
    public List<GameObject> boxes;

    public GameObject[] firstStep;
    public GameObject[] secondStep;
    public GameObject[] thirdStep;
    public GameObject[] fourStep;
    public GameObject[] fiveStep;

    public float firstStar;
    public float secondStar;
    public float thirdStar;

    public int life;        // 떨어져도 되는 상자수 상한

    [Header("Wind Settings")]
    public bool useWind = false;  // 바람 설정 사용할지 여부
    public bool random = false;     // 랜덤 바람일지
    public WindManager.WindType windType;
    public WindManager.WindSpeed windSpeed;
}
