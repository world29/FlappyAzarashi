using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyEventInterval : MonoBehaviour
{
    public float m_interval = 10;
    public UnityEvent m_event;

    Animator m_animator;
    Coroutine m_coroutine;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle") && m_coroutine == null)
        {
            m_coroutine = StartCoroutine(BodyAttackCoroutine(m_interval));
        }
    }

    IEnumerator BodyAttackCoroutine(float interval)
    {
        float t = 0;

        while (true)
        {
            t += Time.deltaTime;

            if (t >= interval)
            {
                t = 0;

                m_event.Invoke();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
