using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Courier : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator.SetTrigger("Hello");
    }

    public void ReHello()
    {
        StartCoroutine(ReHelloDelay());
    }

    IEnumerator ReHelloDelay()
    {
        yield return new WaitForSeconds(6);

        animator.SetTrigger("Hello");
    }
}
