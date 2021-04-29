using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public Transform m_cameraTransform;

    public float m_knockbackDuration = 1;

    private Vector3 m_lastCameraPosition = Vector3.zero;

    private Vector3 m_knockbackPosition;

    private Coroutine m_coroutineHandle = null;

    private bool m_initialized = false;

    private void LateUpdate()
    {
        if (m_cameraTransform == null) return;

        if (!m_initialized) return;

        Vector2 targetMovement = m_cameraTransform.position - m_lastCameraPosition;

        m_knockbackPosition += (Vector3)targetMovement;

        m_lastCameraPosition = m_cameraTransform.position;
    }

    public void SaveKnockbackPosition()
    {
        SetKnockbackPosition(transform.position);
    }

    void SetKnockbackPosition(Vector3 position)
    {
        m_knockbackPosition = position;

        m_lastCameraPosition = m_cameraTransform.position;

        m_initialized = true;
    }

    public void StartKnockback()
    {
        if (m_coroutineHandle != null) return;

        m_coroutineHandle = StartCoroutine(KnockbackCoroutine(m_knockbackDuration));
    }

    IEnumerator KnockbackCoroutine(float duration)
    {
        // ノックバックによる移動量を求める
        var moveAmount = m_knockbackPosition - transform.position;
        var steps = duration / Time.deltaTime;
        moveAmount /= steps;

        float t = 0;

        while (t < duration)
        {
            transform.position += moveAmount;

            yield return null;

            t += Time.deltaTime;
        }

        m_coroutineHandle = null;
    }
}
