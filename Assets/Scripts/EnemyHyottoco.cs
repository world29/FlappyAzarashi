using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHyottoco : MonoBehaviour
{
    public Animator m_animator;

    GameObject m_player;
    bool m_detected = false;

    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (m_detected)
        {
            var rootObject = m_animator.gameObject;

            var offsetX = m_player.GetComponent<Rigidbody2D>().velocity.x * Time.deltaTime;
            var pos = rootObject.transform.position;

            pos.x += offsetX;

            rootObject.transform.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_animator.SetTrigger("appear");

        m_detected = true;
    }
}
