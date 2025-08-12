using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveItemText : MonoBehaviour
{
    public TextMeshProUGUI SpeechBubble;

    TouchOutline target;

    void OnEnable()
    {
        SpeechBubble.text = "Touch Box !";
        target = null; 
    }


    void Update()
    {
        if (target != null)
        {
            if (!target.isOutlined)
            {
                SpeechBubble.text = "Touch Box !";
                target = null;
            }

        }
        else if (BoxManager.Instance != null)
        {
            foreach (GameObject box in BoxManager.Instance.GoaledBoxes)
            {
                target = box.GetComponent<TouchOutline>();

                if (target.isOutlined)
                {
                    SpeechBubble.text = "Press Button Again !";
                    return;
                }
                else
                    target = null;


            }
        }

    }
}
