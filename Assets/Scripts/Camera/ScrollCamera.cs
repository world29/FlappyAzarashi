using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour
{
    public GameObject m_target;
    public float m_offsetX;

    public float m_smoothTime = 0.3f;
    public float m_maxSpeed = 10;

    private Vector3 m_velocity = Vector3.zero;

    private void Update()
    {
        if (Application.isEditor)
        {
            UpdateCameraPosition();
        }
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        if (m_target == null)
        {
            return;
        }

        var targetPos = new Vector3(m_target.transform.position.x + m_offsetX, transform.position.y, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_velocity, m_smoothTime, m_maxSpeed);
    }
}
