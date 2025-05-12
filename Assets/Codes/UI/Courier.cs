using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Courier : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
            animator.SetTrigger("Hello");
        else
            animator.SetTrigger("Idle");
    }

    /*void Update()
    {
        
    }*/

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
    }
}
