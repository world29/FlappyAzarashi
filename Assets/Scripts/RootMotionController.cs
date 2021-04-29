using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour
{
    private Animator m_animator;

    private Vector3 m_totalDeltaPosition;
    private Quaternion m_totalDeltaRotation;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (m_totalDeltaPosition.magnitude > 0)
        {
            transform.localPosition += m_totalDeltaPosition;

            m_totalDeltaPosition = Vector3.zero;
        }

        if (m_totalDeltaRotation != Quaternion.identity)
        {
            transform.localRotation *= m_totalDeltaRotation;

            m_totalDeltaRotation = Quaternion.identity;
        }
    }

    private void OnAnimatorMove()
    {
        m_totalDeltaPosition += m_animator.deltaPosition;
        m_totalDeltaRotation *= m_animator.deltaRotation;
    }
}
