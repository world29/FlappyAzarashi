using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float m_speed = 5.0f;
    public float m_lifetime = 1.0f;

    private void Update()
    {
        // 移動
        var localMove = (m_speed * Time.deltaTime) * Vector3.right;

        var globalMove = transform.rotation * localMove;

        transform.Translate(globalMove);

        // 寿命
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_lifetime < 0)
        {
            return;
        }

        Destroy(gameObject);
    }
}
