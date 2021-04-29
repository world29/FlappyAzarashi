using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAt : MonoBehaviour
{
    public SpriteRenderer m_spriteRenderer;

    public GameObject m_lookAtTarget;

    private Quaternion m_savedRotation;

    [Range(0, 90)]
    public float m_lookatRotationLimit = 30;
    public float m_lookatRotationSpeed = 10;

    private void Awake()
    {
        m_savedRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        if (m_lookAtTarget != null)
        {
            Quaternion targetRotation = Quaternion.identity;

            Vector3 enemyDirection = Vector3.left;
            var toPlayer = m_lookAtTarget.transform.position - transform.position;

            var angle = Vector3.SignedAngle(enemyDirection, toPlayer, Vector3.forward);

            bool inSight = (angle < m_lookatRotationLimit && angle > -m_lookatRotationLimit);
            if (inSight)
            {
                targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            var step = m_lookatRotationSpeed * Time.deltaTime;
            m_savedRotation = Quaternion.RotateTowards(m_savedRotation, targetRotation, step);

            m_spriteRenderer.transform.rotation *= m_savedRotation;
        }
    }
}
