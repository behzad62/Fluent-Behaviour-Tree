using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Decorator node that always succeeds.
    /// </summary>
    public class SucceederNode<T> : ParentBehaviourTreeNode<T> where T : ITickData
    {

        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private BehaviourTreeNode<T> ChildNode => ChildCount == 0 ? null : this[0];

        public SucceederNode(string name, int id) : base(name, id) { }

        protected override Status AbstractTick(T data)
        {
            if (ChildNode == null)
                throw new ApplicationException("InverterNode must have a child node!");

            ChildNode.Tick(data);

            return Status.Success;
        }
        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public override void AddChild(BehaviourTreeNode<T> child)
        {
            if (this.ChildNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to SucceederNode!");
            }

            base.AddChild(child);
        }
    }
    public class SucceederNode : SucceederNode<TimeData>
    {
        public SucceederNode(string name, int id) : base(name, id) { }
    }
}
