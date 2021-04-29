using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOtafukuSpawnParticle : MonoBehaviour
{
    public GameObject m_particle;

    public void Play()
    {
        var go = GameObject.Instantiate(m_particle, transform.position, Quaternion.identity);

        var constraint = go.AddComponent<PositionConstraintObject>();

        go.GetComponent<ParticleSystem>().Play();
    }
}
