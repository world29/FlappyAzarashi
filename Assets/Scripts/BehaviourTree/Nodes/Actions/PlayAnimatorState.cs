using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Action/PlayAnimatorState")]
    public class PlayAnimatorState : Action
    {
        public string m_stateName;

        private bool m_isPlaying;
        private float m_normalizedTime;

        // override XNode.Init()
        protected override void Init()
        {
            base.Init();

            m_isPlaying = false;
        }

        protected override void OnReady()
        {
            base.OnReady();

            Debug.Log("PlayAnimationState.OnReady");

            m_isPlaying = false;
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            m_nodeStatus = EvaluateImpl(go);

            return m_nodeStatus;
        }

        private NodeStatus EvaluateImpl(GameObject go)
        {
            var animator = go.GetComponent<Animator>();

            if (!m_isPlaying)
            {
                animator.Play(m_stateName);

                m_isPlaying = true;

                return NodeStatus.RUNNING;
            }

            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName(m_stateName))
            {
                m_isPlaying = false;

                return NodeStatus.SUCCESS;
            }

            m_normalizedTime = stateInfo.normalizedTime;

            return NodeStatus.RUNNING;
        }

        public override float GetProgress()
        {
            return m_isPlaying ? m_normalizedTime : 0;
        }
    }
}
