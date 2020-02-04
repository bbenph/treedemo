using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using treedemo;

namespace NUnitTestProject1
{
    public class AgentRelationshipService_Test
    {
        private IAgentRelationshipService agentRelationshipServices;
        DI dI = new DI();

        [SetUp]
        public void Setup()
        {
            var container = dI.DICollections();

            agentRelationshipServices = container.Resolve<IAgentRelationshipService>();

        }

        [Test]
        public void 模拟点数据()
        {
            var root = "root";
            agentRelationshipServices.Create(root, string.Empty);

            for (int i = 0; i < 10; i++)
            {
                var u = "child" + i;
                agentRelationshipServices.Create(u, "root");
                for (int j = 0; j < 10; j++)
                {
                    var uu = u + "_grandson" + j;
                    agentRelationshipServices.Create(uu, u);
                }
            }

            Assert.True(true);
        }

        [Test]
        public void 创建根节点()
        {
            var user = "root";
            agentRelationshipServices.Create(user, string.Empty);
            Assert.True(true);
        }

        [Test]
        public void 创建非根节点()
        {
            var user = "child1";
            agentRelationshipServices.Create(user, "root");
            Assert.True(true);
        }

        [Test]
        public void 移动child1为根节点()
        {
            var user = "child6";
            agentRelationshipServices.MakeRoot(user);
            Assert.True(true);
        }

        [Test]
        public void 移动child5为到child2()
        {
            var user = "child6";
            var to = "child2";
            agentRelationshipServices.MoveTo(user, to);
            Assert.True(true);
        }

        [Test]
        public void 获取所有后代()
        {
            var user = "child2";
            List<string> list = agentRelationshipServices.GetDescendants(user);
            Assert.True(list.Count > 0);
        }

        [Test]
        public void 获取所有祖先()
        {
            var user = "child6_grandson2";
            List<string> list = agentRelationshipServices.GetAncestors(user, true);
            Assert.True(list.Count > 0);
        }

        [Test]
        public void 获取直接下级()
        {
            var user = "child2";
            List<string> list = agentRelationshipServices.GetChildren(user);
            Assert.True(list.Count > 0);
        }

        [Test]
        public void 获取直接上级()
        {
            var user = "child2";
            string p = agentRelationshipServices.GetParent(user);
            Assert.True(true);
        }

        [Test]
        public void 获取祖先()
        {
            var user = "child6_grandson0";
            string p = agentRelationshipServices.GetRoot(user);
            Assert.True(true);
        }

        [Test]
        public void 获取所有兄弟姐妹()
        {
            var user = "child6";
            List<string> list = agentRelationshipServices.GetSiblings(user, false);
            Assert.True(true);
        }

        [Test]
        public void 获取所有根()
        {
            var user = "child6";
            List<string> list = agentRelationshipServices.GetRoots();
            Assert.True(true);
        }

        [Test]
        public void 从当前节点生成树()
        {
            var user = "child2";
            var tree = agentRelationshipServices.GetTree(user);
            Assert.True(true);
        }


        [Test]
        public void 根节点数()
        {
            var user = "child1";
            var trees = agentRelationshipServices.GetRootTree();
            Assert.True(true);
        }
    }
}
