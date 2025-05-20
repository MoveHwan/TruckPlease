using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCourier : MonoBehaviour
{
    public BoxDeleteUI BoxDeleteUI;

    public Animator DeleteCourierAni;
    public GameObject CarryingBox;

    BoxManager BoxManager;

    AnimatorStateInfo animStateInfo;

    Vector3 defaultVec, targetVec;

    float targetPos;
    bool courierMoveOn, isCarryOn;

    void Start()
    {
        BoxManager = BoxManager.Instance;

        defaultVec = transform.position;
        targetVec = Vector3.right * -defaultVec.x + Vector3.up * defaultVec.y + Vector3.forward * defaultVec.z;

        gameObject.SetActive(false);
    }


    void Update()
    {
        if (courierMoveOn)
        {
            if (transform.position == targetVec)
            {
                transform.position = targetVec;
                courierMoveOn = false;

                if (targetVec.x == -defaultVec.x)
                {
                    End();
                }
                return;
            }

            if (!isCarryOn && transform.position.x <= targetPos + 0.5f)
            {
                isCarryOn = true;

                CarryingBoxOn();
            }

            transform.position = Vector3.MoveTowards(transform.position, targetVec, Time.deltaTime * 3);
        }
    }


    public void CarryingStart()
    {
        gameObject.SetActive(true);

        GameObject targeBox = BoxManager.GoaledBoxes[BoxManager.GoaledBoxes.Count - 1];

        CarryingBox.GetComponent<MeshFilter>().sharedMesh = targeBox.GetComponent<MeshFilter>().sharedMesh;
        CarryingBox.GetComponent<MeshRenderer>().sharedMaterials = targeBox.GetComponent<MeshRenderer>().sharedMaterials;
        CarryingBox.transform.localScale = targeBox.GetComponent<Transform>().localScale;

        targetPos = targeBox.transform.position.x;

        CarryingBox.SetActive(false);

        DeleteCourierAni.SetTrigger("DeleteBoxStart");

        courierMoveOn = true;
    }

    void CarryingBoxOn()
    {
        DeleteCourierAni.SetTrigger("CarryBox");

        CarryingBox.SetActive(true);

        BoxManager.DeleteBox();
    }


    void End()
    {
        transform.position = defaultVec;

        courierMoveOn = false;
        isCarryOn = false;

        BoxDeleteUI.DeleteEnd();

        gameObject.SetActive(false);
    }

}
