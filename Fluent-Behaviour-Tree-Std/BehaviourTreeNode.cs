﻿namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public abstract class BehaviourTreeNode<T> where T : ITickData
    {
        public static event System.EventHandler<string> Ticked;
        public bool IsCondition { get; set; }

        public bool IsDisabled { get; set; }

        /// <summary>
        /// The reference name for this node
        /// </summary>
        public string Name { get; }

        public int Id { get; set; }

        /// <summary>
        /// The result of the last execution
        /// </summary>
        public Status LastExecutionStatus { get; private set; }

        /// <summary>
        /// Used to determine if this node has been ticked yet this iteration.
        /// </summary>
        public bool HasExecuted { get; private set; }

        protected BehaviourTreeNode(string name, int id)
        {
            Name = name;
            Id = id;
            LastExecutionStatus = Status.Failure;
        }

        protected BehaviourTreeNode()
        {
            Name = "BehaviourTreeNode";
            LastExecutionStatus = Status.Failure;
        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public virtual Status Tick(T time)
        {
            if (IsDisabled)
                return Status.Invalid;
            //BeforeTick();
            ResetLastExecStatus();
            LastExecutionStatus = AbstractTick(time);
            HasExecuted = true;
            OnTicked();
            return LastExecutionStatus;
        }

        private void BeforeTick()
        {
            Ticked?.Invoke(this, $"Node #{Id} - {Name} is going to tick");
        }
        private void OnTicked()
        {
            if (LastExecutionStatus == Status.Failure || LastExecutionStatus == Status.Success)
                Ticked?.Invoke(this, $"Node #{Id} - {Name} returned status: {LastExecutionStatus}");
        }

        /// <summary>
        /// Marks that this node hasn't executed yet.
        /// </summary>
        internal virtual void ResetLastExecStatus()
        {
            HasExecuted = false;
        }

        protected abstract Status AbstractTick(T data);
    }

    public abstract class BehaviourTreeNode : BehaviourTreeNode<TimeData> { }
}
