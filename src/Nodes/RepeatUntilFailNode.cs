using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Repeats child node until it returns a failure
    /// </summary>
    public class RepeatUntilFailNode<T> : ParentBehaviourTreeNode<T> where T : ITickData
    {

        /// <summary>
        /// The child node to be repeated
        /// </summary>
        private BehaviourTreeNode<T> ChildNode => ChildCount == 0 ? null : this[0];

        public RepeatUntilFailNode(string name, int id) : base(name, id) { }


        protected override Status AbstractTick(T data)
        {
            if (ChildNode == null)
                throw new ApplicationException("InverterNode must have a child node!");

            var childStatus = ChildNode.Tick(data);

            if (childStatus == Status.Failure)
                return Status.Success;

            return Status.Running;
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
}
