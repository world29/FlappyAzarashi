using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool m_lookatPlayer;
    [Range(0, 90)]
    public float m_lookatRotationLimit = 30;
    public float m_lookatRotationSpeed = 10;

    public EnemyShot m_shotAbility;

    bool m_active = false;
    GameObject m_player;

    private void Awake()
    {
        gameObject.SetActive(m_active);
    }

    private void Update()
    {
        if (!m_active) return;

        // プレイヤーのほうを向く
        if (m_lookatPlayer)
        {
            Quaternion targetRotation = Quaternion.identity;

            Vector3 enemyDirection = Vector3.left;
            var toPlayer = m_player.transform.position - transform.position;

            var angle = Vector3.SignedAngle(enemyDirection, toPlayer, Vector3.forward);

            bool inSight = (angle < m_lookatRotationLimit && angle > -m_lookatRotationLimit);
            if (inSight)
            {
                targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            var step = m_lookatRotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var contact = collision.contacts[0];
            gameObject.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);

            gameObject.SetActive(false);

            return;
        }

        gameObject.SetActive(false);
    }

    public void Activate()
    {
        m_active = true;

        m_player = GameObject.FindWithTag("Player");

        gameObject.SetActive(m_active);
    }

    public void Deactivate()
    {
        m_active = false;

        gameObject.SetActive(m_active);
    }

    public void Shot()
    {
        if (m_shotAbility != null)
        {
            m_shotAbility.gameObject.SendMessage("Shot");
        }
    }
}
