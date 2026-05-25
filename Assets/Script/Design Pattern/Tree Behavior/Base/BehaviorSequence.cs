using System.Collections.Generic;

namespace Script.Design_Pattern.Tree_Behavior.Base
{
    public class BehaviorSequence : BehaviorNode
    {
        private List<BehaviorNode> nodes = new List<BehaviorNode>();

        public BehaviorSequence(List<BehaviorNode> nodes)
        {
            this.nodes = nodes;
        }
        
        public override NodeState Evaluate()
        {
            var isAnyChildIsRunning = false;
            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        isAnyChildIsRunning = true;
                        continue;
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return state;
                }
            }

            state = isAnyChildIsRunning ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
}
