using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool m_lookatPlayer;
    [Range(0, 90)]
    public float m_lookatRotationLimit = 30;
    public float m_lookatRotationSpeed = 10;
    public AudioClip m_breakSound;

    public EnemyShot m_shotAbility;

    bool m_active = false;
    GameObject m_player;
    AudioSource m_audioSource;
    SpriteRenderer m_spriteRenderer;
    Animator m_animator;

    private void Awake()
    {
        gameObject.SetActive(m_active);
        m_audioSource = GetComponent<AudioSource>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
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
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerDashAttack"))
        {
            StartCoroutine(BreakEnemyCoroutine(collision));
        }
    }

    IEnumerator BreakEnemyCoroutine(Collision2D collision)
    {
        var contact = collision.contacts[0];
        gameObject.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);

        //AudioSource.PlayClipAtPoint(m_breakSound, transform.position);
        m_audioSource.clip = m_breakSound;
        m_audioSource.Play();

        m_spriteRenderer.enabled = false;
        m_animator.enabled = false;
        var colliders = GetComponentsInChildren<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        yield return new WaitForSecondsRealtime(m_breakSound.length);

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
