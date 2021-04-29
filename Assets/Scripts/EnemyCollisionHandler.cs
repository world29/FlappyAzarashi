using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollisionHandler : MonoBehaviour
{
    public GameObject m_enemyRoot;
    public GameObject m_enemySprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("PlayerDashAttack"))
        {
            StartCoroutine(BreakEnemyCoroutine(collision));
        }
    }

    IEnumerator BreakEnemyCoroutine(Collision2D collision)
    {
        if (m_enemySprite)
        {
            var contact = collision.contacts[0];

            m_enemySprite.SendMessage("BreakSprite", contact.point, SendMessageOptions.DontRequireReceiver);
        }

        m_enemyRoot.SetActive(false);

        yield return null;
    }
}
