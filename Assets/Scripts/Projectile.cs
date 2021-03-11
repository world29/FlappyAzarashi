using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform m_transform;

    Rigidbody2D m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void ApplyAngle(float offsetAngleInDegree)
    {
        float targetAngle = targetAngle = Mathf.Atan2(m_rigidbody.velocity.y, m_rigidbody.velocity.x) * Mathf.Rad2Deg;

        m_transform.localRotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
    }
}
