using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindUI : MonoBehaviour
{
    public RectTransform Arrow;
    public TextMeshProUGUI WindStrengthText;

    Vector3[] arrowDirs = { new(0, 0, 50), new(-30, 0, 0), new(0, 0, -50) };

    Vector3 targetVec;

    int windStrength;
    int idx = 1;

    void Start()
    {
        Arrow.rotation = Quaternion.Euler(arrowDirs[1]);
    }

    
    void Update()
    {
        /*if (WindManager.Instance.IsWindEnabled)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);

            if (int.Parse(WindStrengthText.text) != windStrength)
            {
                WindStrengthText.text = windStrength.ToString();
            }

            WindDirSet_Update();
        }
        else
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }*/

    }

    void WindDirSet_Update()
    {
        Vector3 adjustedEuler = new Vector3(
           Arrow.eulerAngles.x > 180f ? Arrow.eulerAngles.x - 360f : Arrow.eulerAngles.x,
           Arrow.eulerAngles.y > 180f ? Arrow.eulerAngles.y - 360f : Arrow.eulerAngles.y,
           Arrow.eulerAngles.z > 180f ? Arrow.eulerAngles.z - 360f : Arrow.eulerAngles.z
       );

        if (arrowDirs[idx] == adjustedEuler)
            return;
        

        Arrow.eulerAngles = Vector3.MoveTowards(adjustedEuler, arrowDirs[idx], Time.deltaTime * 2);
    }
}
