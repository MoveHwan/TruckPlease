using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem particle;  // �ν����Ϳ��� �����ϰų� �ڵ忡�� ã�� �� ����
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
