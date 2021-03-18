using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMoving : MonoBehaviour
{
    public Transform[] m_controlPoints;

    [Range(0, 1)]
    public float m_time = 0;

    private void Update()
    {
        // Animator で制御されるパラメータ[t]をもとに位置を更新する
        UpdatePositionByTime(m_time);
    }

    void UpdatePositionByTime(float t)
    {
        t = Mathf.Clamp01(t);

        int segments = m_controlPoints.Length / 3;
        float time_per_segment = 1.0f / segments;

        // current segment
        int s = Mathf.FloorToInt(t / time_per_segment);
        float time_in_segment = (t % time_per_segment) / time_per_segment;

        if (s < segments)
        {
            transform.position = CalculateBezier(
                m_controlPoints[s * 3 + 0].position,
                m_controlPoints[s * 3 + 1].position,
                m_controlPoints[s * 3 + 2].position,
                m_controlPoints[s * 3 + 3].position,
                time_in_segment);
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
