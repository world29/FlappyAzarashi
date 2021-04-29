using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementFollowTarget : MonoBehaviour, IEnemyMovement
{
    public GameObject m_target;

    public float m_speed = 1;

    public void StartMovement()
    {
    }

    public void UpdateMovement()
    {
        if (m_target == null)
        {
            return;
        }

        var delta = m_speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position, delta);
    }
}
