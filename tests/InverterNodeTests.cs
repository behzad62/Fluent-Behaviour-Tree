﻿using System;
using FluentBehaviourTree;
using Moq;
using Xunit;

namespace tests
{
    public class InverterNodeTests
    {
        InverterNode testObject;

        void Init()
        {
            testObject = new InverterNode("some-node", 0);
        }

        [Fact]
        public void ticking_with_no_child_node_throws_exception()
        {
            Init();

            Assert.Throws<ApplicationException>(
                () => testObject.Tick(new TimeData(0))
            );
        }

        [Fact]
        public void inverts_success_of_child_node()
        {
            Init();

            var time = new TimeData(0);

            var mockChildNode = new Mock<BehaviourTreeNode>();
            mockChildNode
                .Setup(m => m.Tick(time))
                .Returns(Status.Success);

            testObject.AddChild(mockChildNode.Object);

            Assert.Equal(Status.Failure, testObject.Tick(time));

            mockChildNode.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void inverts_failure_of_child_node()
        {
            Init();

            var time = new TimeData(0);

            var mockChildNode = new Mock<BehaviourTreeNode>();
            mockChildNode
                .Setup(m => m.Tick(time))
                .Returns(Status.Failure);

            testObject.AddChild(mockChildNode.Object);

            Assert.Equal(Status.Success, testObject.Tick(time));

            mockChildNode.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void pass_through_running_of_child_node()
        {
            Init();

            var time = new TimeData(0);

            var mockChildNode = new Mock<BehaviourTreeNode>();
            mockChildNode
                .Setup(m => m.Tick(time))
                .Returns(Status.Running);

            testObject.AddChild(mockChildNode.Object);

            Assert.Equal(Status.Running, testObject.Tick(time));

            mockChildNode.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void adding_more_than_a_single_child_throws_exception()
        {
            Init();

            var mockChildNode1 = new Mock<BehaviourTreeNode>();
            testObject.AddChild(mockChildNode1.Object);

            var mockChildNode2 = new Mock<BehaviourTreeNode>();
            Assert.Throws<ApplicationException>(() => 
                testObject.AddChild(mockChildNode2.Object)
            );
        }


    }
}
