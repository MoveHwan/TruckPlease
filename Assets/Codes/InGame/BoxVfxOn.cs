using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem particle;  // �ν����Ϳ��� �����ϰų� �ڵ忡�� ã�� �� ����

    public void ParticlePlay()
    {
        particle.Play();
    }
}
