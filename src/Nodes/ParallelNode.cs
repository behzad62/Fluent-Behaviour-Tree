﻿namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode<T> : ParentBehaviourTreeNode<T> where T : ITickData
    {
        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private readonly int _numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private readonly int _numRequiredToSucceed;

        public ParallelNode(string name, int id, int numRequiredToFail, int numRequiredToSucceed) : base(name, id)
        {
            _numRequiredToFail = numRequiredToFail;
            _numRequiredToSucceed = numRequiredToSucceed;
            if (numRequiredToFail + numRequiredToSucceed > ChildCount)
                throw new System.ArgumentException("Number of succeeds and fails cannot be larger than number of children.");
        }

        protected override Status AbstractTick(T data)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            for (int i = 0; i < ChildCount; i++)
            {
                var child = this[i];
                var childStatus = child.Tick(data);
                switch (childStatus)
                {
                    case Status.Success: ++numChildrenSuceeded; break;
                    case Status.Failure: ++numChildrenFailed; break;
                }
            }
            
            if (_numRequiredToSucceed > 0 && numChildrenSuceeded >= _numRequiredToSucceed)
            {
                return Status.Success;
            }
            
            if (_numRequiredToFail > 0 && numChildrenFailed >= _numRequiredToFail)
            {
                return Status.Failure;
            }
            
            return Status.Running;
        }
    }

    public class ParallelNode : ParallelNode<TimeData>
    {
        public ParallelNode(string name, int id, int numRequiredToFail, int numRequiredToSucceed) : base(name, id, numRequiredToFail, numRequiredToSucceed) { }
    }
}
