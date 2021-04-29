using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionConstraintObject : MonoBehaviour
{
    private Transform m_target;

    Vector3 m_lastTargetPosition = Vector3.zero;

    private bool m_initialized = false;

    private void Awake()
    {
        m_target = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (m_target == null) return;

        // 初回の呼び出し
        if (!m_initialized)
        {
            m_lastTargetPosition = m_target.position;

            m_initialized = true;

            return;
        }

        Vector2 targetMovement = m_target.position - m_lastTargetPosition;

        transform.position += (Vector3)targetMovement;

        m_lastTargetPosition = m_target.position;
    }
}
