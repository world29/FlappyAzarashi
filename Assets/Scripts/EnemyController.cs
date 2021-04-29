using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public ActorId m_actorId;

    public int m_hitPoint = 3;

    public string m_damageSe;
    public string m_breakSe;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Assert(gameObject.activeSelf);

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerDashAttack"))
        {
            m_hitPoint--;

            if (m_hitPoint == 0)
            {
                StartCoroutine(BreakEnemyCoroutine(collision));
            }
            else
            {
                m_animator.SetTrigger("damage");

                Sound.GetInstance().PlaySe(m_damageSe);
            }
        }
    }

    IEnumerator BreakEnemyCoroutine(Collision2D collision)
    {
        var contact = collision.contacts[0];

        BroadcastMessage("BreakSprite", contact.point);

        Sound.GetInstance().PlaySe(m_breakSe);

        BroadcastExecuteEvents.Execute<IActorEvents>(null /* eventData */,
            (handler, eventData) => handler.OnActorDied(m_actorId));

        gameObject.SetActive(false);

        yield return null;
    }
}
