using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxVfxOn : MonoBehaviour
{
    public ParticleSystem particle;  // 인스펙터에서 연결하거나 코드에서 찾을 수 있음

    public void ParticlePlay()
    {
        particle.Play();
    }
}
