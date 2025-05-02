using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBox : MonoBehaviour
{
    Vector3 position;

    void Awake()
    {
        position = transform.position;
    }

    public void ResetPosition()
    {
        ThrowBox throwBox = GetComponent<ThrowBox>();
        throwBox.
        transform.position = position;
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}
