using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMoving : MonoBehaviour
{
    public Transform[] m_controlPoints;

    public bool m_loop = true;
    public float m_speed = 1.0f;

    private Coroutine m_coroutine;

    private void Start()
    {
        m_coroutine = StartCoroutine(MoveCoroutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(m_coroutine);
    }

    IEnumerator MoveCoroutine()
    {
        var segments = m_controlPoints.Length / 3;

        int s = 0;

        while (s < m_controlPoints.Length - 3)
        {
            float t = 0;

            while (t < 1.0f)
            {
                t += Time.deltaTime * m_speed;

                Vector3[] p = {
                    m_controlPoints[s + 0].position,
                    m_controlPoints[s + 1].position,
                    m_controlPoints[s + 2].position,
                    m_controlPoints[s + 3].position,
                };

                transform.position = CalculateBezier(p[0], p[1], p[2], p[3], t);

                yield return new WaitForEndOfFrame();
            }

            s += 3;

            if (s >= (m_controlPoints.Length - 3) && m_loop)
            {
                s = 0;
            }
        }
    }

    Vector3 CalculateBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float tt = t * t;
        float ttt = t * tt;
        float u = 1.0f - t;
        float uu = u * u;
        float uuu = u * uu;

        Vector3 B = new Vector3();
        B = uuu * p0;
        B += 3.0f * uu * t * p1;
        B += 3.0f * u * tt * p2;
        B += ttt * p3;

        return B;
    }

    private void OnDrawGizmos()
    {
        for (int s = 0; s < m_controlPoints.Length - 3; s += 3 )
        {
            for (float t = 0; t <= 1; t += 0.05f)
            {
                Vector3 position = CalculateBezier(
                    m_controlPoints[s + 0].position,
                    m_controlPoints[s + 1].position,
                    m_controlPoints[s + 2].position,
                    m_controlPoints[s + 3].position,
                    t);

                Gizmos.DrawSphere(position, 0.25f);
            }

            Gizmos.DrawLine((Vector2)m_controlPoints[s + 0].position, (Vector2)m_controlPoints[s + 1].position);
            Gizmos.DrawLine((Vector2)m_controlPoints[s + 2].position, (Vector2)m_controlPoints[s + 3].position);
        }
    }
}
