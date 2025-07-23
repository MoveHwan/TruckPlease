using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBox : MonoBehaviour
{
    public static LifeBox instance;

    public GameObject[] BoxLlfes;

    [SerializeField] int life;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        LifeRefresh();
    }

    void LifeRefresh()
    {
        if (life == GameManager.Instance.life) return;

        life = GameManager.Instance.life;

        for (int i = 0; i < BoxLlfes.Length; i++)
        {
            BoxLlfes[i].SetActive(i >= life);
        }

    }
}
