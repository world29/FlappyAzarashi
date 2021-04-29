using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float m_lifetime = 10;

    public float m_gravityScale = 0;

    public Vector2 Velocity { get; set; }

    static readonly float kGravity = 9.8f;

    private void Awake()
    {
        Velocity = Vector2.zero;
    }

    private void Update()
    {
        if (m_gravityScale > 0)
        {
            float dy = kGravity* Time.deltaTime* Time.deltaTime;

            var v = Velocity;

            v.y -= m_gravityScale * dy;

            Velocity = v;
        }
    }

    private void LateUpdate()
    {
        var delta = Velocity * Time.deltaTime;

        transform.position += (Vector3)delta;

        // 寿命
        m_lifetime -= Time.deltaTime;

        if (m_lifetime < 0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
