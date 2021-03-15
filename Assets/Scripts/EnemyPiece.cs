using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : MonoBehaviour
{
    Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    IEnumerator BlinkAndDestroyCoroutine(float blinkingTime)
    {
        m_animator.SetTrigger("blink");

        yield return new WaitForSeconds(blinkingTime);

        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(BlinkAndDestroyCoroutine(1.0f));
    }
}
