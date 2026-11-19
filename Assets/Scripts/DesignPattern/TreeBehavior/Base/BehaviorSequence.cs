using System.Collections.Generic;

namespace DesignPattern.TreeBehavior.Base
{
    public class BehaviorSequence : BehaviorNode
    {
        private List<BehaviorNode> nodes = new();

        public BehaviorSequence(List<BehaviorNode> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            // var isAnyChildRunning = false;
            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        // isAnyChildRunning = true;
                        // continue;
                        state = NodeState.Running;
                        return state;
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;
                }
            }
            state = NodeState.Success;
            // state = isAnyChildRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
}
