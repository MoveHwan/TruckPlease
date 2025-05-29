using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem particle;  // 인스펙터에서 연결하거나 코드에서 찾을 수 있음
    int count = 0;

    public void ParticlePlay()
    {
        if (count == 0)
        { 
            particle.Play();
            count++;
        }

    }
}
