using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DeleteCourier : MonoBehaviour
{
    public static DeleteCourier Instance;

    public InGameItem DeleteItem;

    public Animator DeleteCourierAni;
    public GameObject CarryingBox;
    public Transform hand;

    BoxManager BoxManager;

    BoxCollider carry;

    AnimatorStateInfo animStateInfo;

    Vector3 defaultVec, targetVec;
    [SerializeField] Vector3 boxVec = new(-0.355f, 1.2f, -0.15f);

    float targetPos;
    bool courierMoveOn, isCarryOn;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BoxManager = BoxManager.Instance;

        carry = CarryingBox.GetComponent<BoxCollider>();

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

            if (!isCarryOn && transform.position.x <= targetPos + 0.9f)
            {
                isCarryOn = true;

                CarryingBoxOn();
            }

            transform.position = Vector3.MoveTowards(transform.position, targetVec, Time.deltaTime * 3);


        }
    }


    public void CarryingStart(GameObject targeBox)
    {
        CarryingBox.SetActive(false);
        gameObject.SetActive(true);
        
        CarryingBox.transform.SetParent(transform);

        CarryingBox.GetComponent<MeshFilter>().sharedMesh = targeBox.GetComponent<MeshFilter>().sharedMesh;
        CarryingBox.GetComponent<MeshRenderer>().sharedMaterials = targeBox.GetComponent<MeshRenderer>().sharedMaterials;
        CarryingBox.transform.localScale = targeBox.GetComponent<Transform>().localScale;

        carry.size = targeBox.GetComponent<BoxCollider>().size;

        CarryingBox.transform.GetChild(0).localPosition = new Vector3(-carry.size.x, carry.size.y, -carry.size.z) * 0.5f;

        DeleteCourierAni.SetTrigger("DeleteBoxStart");  

        courierMoveOn = true;
    }

    void CarryingBoxOn()
    {
        DeleteCourierAni.SetTrigger("CarryBox");

        StartCoroutine(BoxDelay());
    }

    IEnumerator BoxDelay()
    {
        yield return new WaitForSeconds(0.15f);

        CarryingBox.transform.localPosition = boxVec;
        
        CarryingBox.transform.position = CarryingBox.transform.GetChild(0).position;

        CarryingBox.transform.SetParent(hand.transform);

        CarryingBox.SetActive(true);

        BoxManager.Instance.ActiveKeepItem();

        yield break;
    }


    void End()
    {
        transform.position = defaultVec;

        courierMoveOn = false;
        isCarryOn = false;

        DeleteItem.DeleteEnd();

        DeleteCourierAni.SetTrigger("Idle");

        gameObject.SetActive(false);
    }

}
