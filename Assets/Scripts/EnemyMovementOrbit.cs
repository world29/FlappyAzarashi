using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementOrbit : MonoBehaviour
{
    public Transform m_orbitCenter;

    public Vector2 m_orbitRadius;

    public float m_speed = 1;

    float m_time = 0;

    static readonly float kPI2 = 2 * Mathf.PI;

    private void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        float t = m_time * kPI2;

        t *= m_speed;
        Vector2 coord = new Vector2(m_orbitRadius.x * Mathf.Cos(-t), m_orbitRadius.y * Mathf.Sin(-t));

        transform.position = m_orbitCenter.position + (Vector3)coord;

        m_time += Time.deltaTime;
    }
}
