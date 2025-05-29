using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    ThrowBox throwBox;
    public Rigidbody rb;

    Vector3 windRight = new Vector3(1f, 0f, 0f); // 오른쪽
    Vector3 windLeft = new Vector3(-1f, 0f, 0f); // 왼쪽
    Vector3 windFront = new Vector3(0f, 0f, 1f); // 앞쪽

    Vector3 windDir;
    float windSpeed;

    float windForceWeak = 0.07f;
    float windForceMiddle = 0.12f;
    float windForceStrong = 0.18f;

    void Awake()
    {
        throwBox = GetComponent<ThrowBox>();
    }

    void FixedUpdate()
    {
        WindCome();
    }

    void WindCome()
    {
        if (rb == null)
            return;
        if (throwBox.canWind && WindManager.instance.IsWindEnabled)
        {
            switch (WindManager.instance.windType)
            {
                case WindManager.WindType.left:
                    windDir = windLeft;
                    break;
                case WindManager.WindType.right:
                    windDir = windRight; break;
                case WindManager.WindType.front:
                     windDir = windFront; break;
            }
            switch (WindManager.instance.windSpeed)
            {
                case WindManager.WindSpeed.weak:
                    windSpeed = windForceWeak;
                    break;
                case WindManager.WindSpeed.middle:
                    windSpeed = windForceMiddle; break;
                case WindManager.WindSpeed.strong:
                    windSpeed = windForceStrong; break;
            }

            rb.AddForce(windDir.normalized * windSpeed, ForceMode.Impulse);
        }
    }
}
