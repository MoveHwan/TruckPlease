using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager instance;

    public enum WindType { left, front, right }
    public enum WindSpeed { weak, middle, strong }

    [Header("�ٶ� ����")]
    public WindType windType;
    public WindSpeed windSpeed;

    [Header("���� �ٶ� �ɼ�")]
    public bool useRandomWind = false;
    public float windChangeInterval = 5f; // ���� �ٶ� ���� �ֱ�

    private float timer;

    private bool isWindEnabled = true;

    public bool IsWindEnabled => isWindEnabled;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!useRandomWind || !isWindEnabled) return;

        timer += Time.deltaTime;
        if (timer >= windChangeInterval)
        {
            timer = 0f;
            SetRandomWind();
        }
    }

    /// <summary>
    /// ���� �ٶ� ����
    /// </summary>
    void SetRandomWind()
    {
        windType = (WindType)Random.Range(0, System.Enum.GetValues(typeof(WindType)).Length);
        windSpeed = (WindSpeed)Random.Range(0, System.Enum.GetValues(typeof(WindSpeed)).Length);
        Debug.Log($"[Wind] Type: {windType}, Speed: {windSpeed}");
    }

    /// <summary>
    /// �ܺο��� �ٶ� ���� ����
    /// </summary>

    public void DisableWind()
    {
        isWindEnabled = false;
        useRandomWind = false;
        Debug.Log("[WindManager] Wind disabled for this stage.");
    }

    public void SetFixedWind(WindType type, WindSpeed speed)
    {
        isWindEnabled = true;
        useRandomWind = false;
        windType = type;
        windSpeed = speed;
        Debug.Log($"[Fixed Wind] Type: {windType}, Speed: {windSpeed}");
    }

    public void RandomWind()
    {
        isWindEnabled = true;
        useRandomWind = true;
        Debug.Log($"[Fixed Wind] Type: {windType}, Speed: {windSpeed}");
        SetRandomWind();
    }

}
