using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Courier : MonoBehaviour
{
    public Transform courier;
    public Animator animator;

    public bool isClear, isPush;

    AnimatorStateInfo animStateInfo;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
            animator.SetTrigger("Hello");
        else
        {
            animator.SetTrigger("Idle");
        }
        //PushStart();
    }

    void Update()
    {
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("Push") && isPush)
        {
            courier.position = Vector3.MoveTowards(courier.position, Vector3.right * -0.669f + Vector3.up * courier.position.y + Vector3.forward * courier.position.z, Time.deltaTime * 1.2f);
        }
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


    public void PushStart()
    {
        animator.SetTrigger("Push");
        isPush = true;
    }

    public void PushStop()
    {
        isPush = false;
    }

    public void Reaction()
    {
        if (isClear)
            animator.SetTrigger("DDabong");
        else
            animator.SetTrigger("Shouting");

    }
}
