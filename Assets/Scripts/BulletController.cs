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
        var newPosition = transform.position;

        newPosition.x += m_speed * Time.deltaTime;

        transform.position = newPosition;

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

        Debug.Log("HIT");
    }
}
