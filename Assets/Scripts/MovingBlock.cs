using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public AnimationCurve m_pattern;
    public float m_time;
    public float m_phaseTime;
    public GameObject root;

    private float m_timer;

    void Start()
    {
        Debug.Assert(m_time > 0);

        m_timer = m_phaseTime;
    }

    private void Update()
    {
        float value = m_pattern.Evaluate(m_timer / m_time);
        float height = Mathf.Lerp(minHeight, maxHeight, value);

        root.transform.localPosition = new Vector3(0.0f, height, 0.0f);

        m_timer += Time.deltaTime;
        if (m_timer > m_time)
        {
            m_timer -= m_time;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var from = transform.position;
        from.y += minHeight;

        var to = transform.position;
        to.y += maxHeight;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(from, to);
    }
#endif
}
