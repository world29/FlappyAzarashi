using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Action/WaitFor")]
    public class WaitFor : Action
    {
        public float m_waitTime = 1;

        private float m_startTime;
        private bool m_isWaiting;

        // override XNode.Init()
        protected override void Init()
        {
            base.Init();

            m_startTime = 0;
        }

        protected override void OnReady()
        {
            base.OnReady();
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            m_nodeStatus = EvaluateImpl(go);

            return m_nodeStatus;
        }

        private NodeStatus EvaluateImpl(GameObject go)
        {
            if (!m_isWaiting)
            {
                m_isWaiting = true;

                m_startTime = Time.timeSinceLevelLoad;

                return NodeStatus.RUNNING;
            }

            float elapsedTime = Time.timeSinceLevelLoad - m_startTime;

            if (elapsedTime > m_waitTime)
            {
                m_isWaiting = false;

                return NodeStatus.SUCCESS;
            }

            return NodeStatus.RUNNING;
        }

        public override float GetProgress()
        {
            return (Time.timeSinceLevelLoad - m_startTime) / m_waitTime;
        }
    }
}
