using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [CreateNodeMenu("BehaviourTree/Decorator/If")]
    public class If : Decorator
    {
        [Tooltip("接続した Conditional ノードが SUCCESS なら実行します。"), Output(ShowBackingValue.Never, ConnectionType.Override)] public Node condition;

        protected Node m_conditionNode;

        public override void Setup()
        {
            base.Setup();

            XNode.NodePort outPort = GetOutputPort("condition");
            if (!outPort.IsConnected)
            {
                return;
            }

            m_conditionNode = outPort.Connection.node as Node;
        }

        public override NodeStatus Evaluate(GameObject go)
        {
            if (m_nodeStatus == NodeStatus.RUNNING || m_conditionNode.Evaluate(go) == NodeStatus.SUCCESS)
            {
                m_nodeStatus = m_node.Evaluate(go);
            }
            else
            {
                m_nodeStatus = NodeStatus.FAILURE;
            }

            return m_nodeStatus;
        }

        //TODO: Decorator 基底クラスに移動する
        public override void CollectConditionals(ref List<Conditional> conditionalNodes)
        {
            base.CollectConditionals(ref conditionalNodes);

            conditionalNodes.Add(m_conditionNode as Conditional);
        }
    }
}
