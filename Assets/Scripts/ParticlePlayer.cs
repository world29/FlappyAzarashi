using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem m_particleSystem;

    public void Play()
    {
        m_particleSystem.Play();
    }
}
