namespace Script.Design_Pattern.Tree_Behavior.Base
{
    public enum NodeState
    {
        Success,
        Running,
        Failure
    }
    
    public abstract class BehaviorNode
    {
        protected NodeState state;
        public NodeState State => state;

        public abstract NodeState Evaluate();
    }
}
