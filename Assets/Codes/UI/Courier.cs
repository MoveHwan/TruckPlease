using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Courier : MonoBehaviour
{
    public Transform courier;
    public Animator animator;
    public Vector3 targetVec;
    public bool isPush, pushOn;

    AnimatorStateInfo animStateInfo;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            animator.SetTrigger("Lean");
            animator.SetTrigger("Hello");
        }
    }

    void Update()
    {
        if (!pushOn && GameManager.Instance && GameManager.Instance.gameEnd)
        {
            pushOn = true;
            StartCoroutine(PushDelay());
        }

        if (gameObject.activeSelf)
        {
            animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (animStateInfo.IsName("Push") && isPush)
            {
                courier.position = Vector3.MoveTowards(courier.position, targetVec, Time.deltaTime * 2);
            }
        }
       
    }

    public void ReHello()
    {
        StartCoroutine(ReHelloDelay());
    }

    IEnumerator ReHelloDelay()
    {
        yield return new WaitForSeconds(10);

        animator.SetTrigger("Hello");
    }


    public void PushStart()
    {
        gameObject.SetActive(true);

        animator.SetTrigger("Push");
        isPush = true;
    }

    IEnumerator PushDelay()
    {
        yield return new WaitForSeconds(0.4f);

        PushStart();
    }

    public void PushStop()
    {
        isPush = false;
    }

    public void Reaction(bool isClear)
    {
        if (isClear)
            animator.SetTrigger("DDabong");
        else
            animator.SetTrigger("Shouting");

    }
}