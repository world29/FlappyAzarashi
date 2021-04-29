using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Action/SendMessage")]
    public class SendMessage : Action
    {
        public string m_methodName;

        // override XNode.Init()
        protected override void Init()
        {
            base.Init();
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
            go.SendMessage(m_methodName);

            return NodeStatus.SUCCESS;
        }

        public override float GetProgress()
        {
            return 0;
        }
    }
}
