using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementTrackingTarget : MonoBehaviour
{
    public Transform m_target;

    public float m_speed = 1;

    private void Update()
    {
        var targetPosition = new Vector3(transform.position.x, m_target.position.y, transform.position.z);

        float maxDelta = m_speed * Time.deltaTime;
        var movedPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDelta);

        var diff = movedPosition - transform.position;
        transform.position += diff;
    }
}
