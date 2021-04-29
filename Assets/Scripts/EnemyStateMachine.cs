using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStateMachine : MonoBehaviour
{
    public UnityEvent m_onActivated = new UnityEvent();
    public UnityEvent m_onDeactivated = new UnityEvent();

    private void Awake()
    {
        // 初期状態は非アクティブとする
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_onActivated.Invoke();
    }

    private void OnDisable()
    {
        m_onDeactivated.Invoke();
    }
}
