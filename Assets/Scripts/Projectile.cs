using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_lifetime = 5;

    private void LateUpdate()
    {
        m_lifetime -= Time.deltaTime;

        if (m_lifetime < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
