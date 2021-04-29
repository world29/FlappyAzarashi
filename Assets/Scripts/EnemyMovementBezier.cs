using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBezier : MonoBehaviour, IEnemyMovement
{
    public Transform[] m_controlPoints;

    public float m_totalTime = 3f;

    private float m_time;

    private Rigidbody2D m_rb;

    private List<Vector3> m_globalControlPoints;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
    }

    public void StartMovement()
    {
        m_time = 0;

        m_globalControlPoints = new List<Vector3>();
        foreach (var cp in m_controlPoints)
        {
            m_globalControlPoints.Add(cp.position);
        }
    }

    public void UpdateMovement()
    {
        var t = m_time / m_totalTime;

        UpdatePositionByTime(t);

        m_time += Time.deltaTime;

        if (m_time > m_totalTime)
        {
            m_time -= m_totalTime;
        }
    }

    void UpdatePositionByTime(float t)
    {
        t = Mathf.Clamp01(t);

        int segments = m_globalControlPoints.Count / 3;
        float time_per_segment = 1.0f / segments;

        // current segment
        int s = Mathf.FloorToInt(t / time_per_segment);
        float time_in_segment = (t % time_per_segment) / time_per_segment;

        if (s < segments)
        {
            var targetPosition = CalculateBezier(
                m_globalControlPoints[s * 3 + 0],
                m_globalControlPoints[s * 3 + 1],
                m_globalControlPoints[s * 3 + 2],
                m_globalControlPoints[s * 3 + 3],
                time_in_segment);

            m_rb.MovePosition(targetPosition);
        }
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
                t += Time.deltaTime;

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

            if (s >= (m_controlPoints.Length - 3))
            {
                break;
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
        for (int s = 0; s < m_controlPoints.Length - 3; s += 3)
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
