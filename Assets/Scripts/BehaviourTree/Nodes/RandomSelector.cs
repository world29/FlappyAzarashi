using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/RandomSelector")]
    public class RandomSelector : Selector
    {
        protected override void OnReady()
        {
            base.OnReady();

            // 再抽選
            m_nodeIndex = Random.Range(0, m_nodes.Count());
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            m_nodeStatus = m_nodes[m_nodeIndex].Evaluate(go);

            return m_nodeStatus;
        }
    }
}
