using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public AI.BehaviourTree m_behaviourTree;

    private AI.BehaviourTree m_behaviourTreeClone;

    private void Start()
    {
        if (m_behaviourTree)
        {
            m_behaviourTreeClone = m_behaviourTree.Copy() as AI.BehaviourTree;
            m_behaviourTreeClone.Setup();
        }
    }

    private void Update()
    {
        if (m_behaviourTreeClone)
        {
            m_behaviourTreeClone.Evaluate(gameObject);
        }
    }
}
