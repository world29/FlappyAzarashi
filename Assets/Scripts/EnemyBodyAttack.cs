using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyAttack : MonoBehaviour, IEnemyMovement
{
    public GameObject m_target;

    public float m_speed = 5;

    Vector3 m_startPosition;
    Vector3 m_targetPosition;

    public void StartMovement()
    {
        m_startPosition = transform.position;
        m_targetPosition = m_target.transform.position;
    }

    public void UpdateMovement()
    {
        if (m_target == null)
        {
            return;
        }

        var diff = m_targetPosition - transform.position;
        float distance = diff.magnitude;

        float dist = distance / (m_targetPosition - m_startPosition).magnitude;

        GetComponent<Rigidbody2D>().velocity = diff.normalized * dist * m_speed;
    }
}
