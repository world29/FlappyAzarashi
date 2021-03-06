﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float m_speed = 5.0f;
    public float m_lifetime = 1.0f;

    public Vector3 Direction { get { return m_direction; } set { m_direction = value.normalized; } }
    Vector3 m_direction;

    private void Awake()
    {
        m_direction = Vector3.right;
    }

    private void Update()
    {
        // 移動
        var movement = m_speed * Direction * Time.deltaTime;
        transform.Translate(movement);

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
