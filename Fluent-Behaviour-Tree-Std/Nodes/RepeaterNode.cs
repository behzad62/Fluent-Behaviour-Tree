using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Repeats the child node for infinity or until max iterations if one is set
    /// </summary>
    public class RepeaterNode<T> : ParentBehaviourTreeNode<T> where T : ITickData
    {
        private int maxIterations = -1;
        private int iterations = 0;

        /// <summary>
        /// The child node to be repeated
        /// </summary>
        private BehaviourTreeNode<T> ChildNode => ChildCount == 0 ? null : this[0];

        public RepeaterNode(string name, int id) : base(name, id) { }

        public RepeaterNode(string name, int id, int maxIterations) : base(name, id)
        {
            this.maxIterations = maxIterations;
        }

        protected override Status AbstractTick(T data)
        {
            if (ChildNode == null)
                throw new ApplicationException("InverterNode must have a child node!");

            if ((maxIterations > -1 || maxIterations == 0) && iterations >= maxIterations)
            {
                iterations = 0;
                return Status.Success;
            }

            ChildNode.Tick(data);
            iterations++;

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
