using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAttackStraight : MonoBehaviour
{
    public Transform m_target;

    public float m_speed = 1;

    [Range(0, 90)]
    public float m_angleSpeed = 15;

    private float m_angle;
    private bool m_moving = false;

    private void Awake()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        StartMovement();
    }

    private void OnDisable()
    {
        StopMovement();
    }

    public void StartMovement()
    {
        Vector2 moveDirection = (m_target.position - transform.position).normalized;

        m_angle = Vector2.SignedAngle(Vector3.right, moveDirection);

        m_moving = true;
    }

    public void StopMovement()
    {
        m_moving = false;
    }

    public void Update()
    {
        if (m_moving)
        {
            var targetDirection = (m_target.position - transform.position).normalized;

            var targetAngle = Vector2.SignedAngle(Vector3.right, targetDirection);

            m_angle = Mathf.MoveTowardsAngle(m_angle, targetAngle, m_angleSpeed * Time.deltaTime);

            var rot = Quaternion.Euler(0, 0, m_angle);

            var delta = (rot * Vector3.right) * m_speed * Time.deltaTime;

            transform.position += delta;
        }
    }
}
