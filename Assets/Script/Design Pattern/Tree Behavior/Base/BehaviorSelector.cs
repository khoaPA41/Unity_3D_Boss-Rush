using System;
using System.Collections.Generic;

namespace Script.Design_Pattern.Tree_Behavior.Base
{
    public class BehaviorSelector : BehaviorNode
    {
        private List<BehaviorNode> nodes = new List<BehaviorNode>();

        public BehaviorSelector(List<BehaviorNode> nodes)
        {
            this.nodes = nodes;
        }
        
        public override NodeState Evaluate()
        {
            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Success:
                        state = NodeState.Success;
                        return state;
                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;
                    case NodeState.Failure:
                        continue;
                }
            }

            state = NodeState.Failure;
            return state;
        }
    }
}
