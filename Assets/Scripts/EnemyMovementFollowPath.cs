using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementFollowPath : MonoBehaviour, IEnemyMovement
{
    public Transform[] m_paths;

    public float m_acceleration = 10;

    private List<Vector2> m_pathsGlobal;

    private int m_pathIndex;
    private float m_distance;

    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_pathsGlobal = new List<Vector2>();

        foreach (var path in m_paths)
        {
            m_pathsGlobal.Add(path.position);
        }

        m_rb = GetComponent<Rigidbody2D>();
    }

    public void StartMovement()
    {
        m_pathIndex = 0;

        var nextIndex = (m_pathIndex + 1) % m_pathsGlobal.Count;

        m_distance = Vector2.Distance(m_pathsGlobal[m_pathIndex], m_pathsGlobal[nextIndex]);
    }

    public void UpdateMovement()
    {
        var destPosition = m_pathsGlobal[m_pathIndex];

        var distance = Vector2.Distance(destPosition, transform.position);
        if (distance > 0.1f)
        {
            var diff = destPosition - (Vector2)transform.position;

            // 目標までの距離が近くなるほど、加速度が弱くなる。
            var acc = Mathf.Lerp(0, m_acceleration, distance / m_distance);

            var delta = diff.normalized * acc * Time.deltaTime;
            m_rb.velocity += delta;
            Debug.DrawLine(transform.position, delta, Color.red);

            //todo: 目標に近づくほど減速する
        }
        else
        {
            // 目標に十分近づいたので次の目標に切り替える
            var nextIndex = (m_pathIndex + 1) % m_pathsGlobal.Count;

            m_distance = Vector2.Distance(m_pathsGlobal[m_pathIndex], m_pathsGlobal[nextIndex]);

            m_pathIndex = nextIndex;
        }
    }

}
