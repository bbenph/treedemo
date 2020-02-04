using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    public class AgentRelationshipService : IAgentRelationshipService
    {
        private readonly IRepository<AgentTree> _agentTreeRepository;
        private readonly IUnitOfWork<AgentContext> _agentUnitOfWork;

        public AgentRelationshipService(
            IRepository<AgentTree> agentTreeRepository,
            IUnitOfWork<AgentContext> agentUnitOfWork
            )
        {
            _agentTreeRepository = agentTreeRepository;
            _agentUnitOfWork = agentUnitOfWork;
        }

        #region 查

        /// <summary>
        /// 获取所有后代
        /// </summary>
        /// <param name="includeSelf"></param>
        public List<string> GetDescendants(string user, bool includeSelf)
        {
            var list = _agentTreeRepository.TableNoTracking.Where(x => x.Ancestor == user);
            if (includeSelf)
            {
                list = list.Where(x => x.Distance >= 0);
            }
            else
            {
                list = list.Where(x => x.Distance > 0);
            }

            var results = list.Select(x => x.Descendant).Distinct();

            return results.ToList();
        }

        /// <summary>
        ///  获取所有祖先
        /// </summary>
        public List<string> GetAncestors(string user, bool includeSelf)
        {
            var list = _agentTreeRepository.TableNoTracking.Where(x => x.Descendant == user);
            if (includeSelf)
            {
                list = list.Where(x => x.Distance >= 0);
            }
            else
            {
                list = list.Where(x => x.Distance > 0);
            }

            var results = list.Select(x => x.Ancestor).Distinct();
            return results.ToList();
        }

        /// <summary>
        /// 获取直接下级
        /// </summary>
        public List<string> GetChildren(string user)
        {
            var list = _agentTreeRepository.TableNoTracking
                .Where(x => x.Ancestor == user && x.Distance == 1);


            var results = list.Select(x => x.Descendant).Distinct();

            return results.ToList();
        }

        /// <summary>
        /// 获取直接上级
        /// </summary>
        public string GetParent(string user)
        {
            var p = _agentTreeRepository.TableNoTracking.FirstOrDefault(x => x.Descendant == user && x.Distance == 1);
            return p?.Ancestor ?? string.Empty;
        }

        /// <summary>
        /// 获取祖先(根)
        /// </summary>
        public string GetRoot(string user)
        {
            var m = _agentTreeRepository.TableNoTracking
                .Where(x => x.Descendant == user).OrderByDescending(x => x.Distance).FirstOrDefault();

            return m?.Ancestor;

        }

        /// <summary>
        /// 获取所有兄弟姐妹(同一个parent)
        /// </summary>
        public List<string> GetSiblings(string user, bool includeSelf)
        {
            var shangji = _agentTreeRepository.TableNoTracking
                .FirstOrDefault(x => x.Descendant == user && x.Distance == 1)?.Ancestor;

            //无上级,即顶级
            if (string.IsNullOrEmpty(shangji))
            {
                return includeSelf ? new List<string>() { user } : new List<string>();
            }

            var children = _agentTreeRepository.TableNoTracking
                .Where(x => x.Ancestor == shangji && x.Distance == 1);

            var results = children.Select(x => x.Descendant).Distinct().ToList();
            if (!includeSelf)
            {
                results.Remove(user);
            }

            return results;
        }


        /// <summary>
        /// 获取所有根
        /// </summary>
        public List<string> GetRoots()
        {
            var roots = from t in _agentTreeRepository.TableNoTracking
                        group t by t.Descendant into g
                        where g.Count() == 1
                        select g.Key;

            var results = roots.Distinct().ToList();
            return results;
        }

        #endregion


        #region 增改

        /// <summary>
        /// 将此节点作为根
        /// </summary>
        public void MakeRoot(string user)
        {
            MoveTo(user);
        }


        /// <summary>
        /// 创建一个新的节点
        /// </summary>
        /// <param name="user"></param>
        /// <param name="parent">默认空,表示创建根节点,否则 创建到parent节点下</param>
        public void Create(string user, string parent = "")
        {
            var existed = _agentTreeRepository.TableNoTracking.Any(x => x.Descendant == user);
            if (existed)
            {
                throw new Exception($"{user}节点已存在");
            }
            if (!string.IsNullOrEmpty(parent))
            {
                var parentexisted = _agentTreeRepository.TableNoTracking.Any(x => x.Descendant == parent);
                if (!parentexisted)
                {
                    throw new Exception($"{parent}父节点不存在");
                }
            }

            var self = new AgentTree
            {
                Ancestor = user,
                Descendant = user,
                Distance = 0,
            };
            if (string.IsNullOrEmpty(parent))
            {
                _agentTreeRepository.Insert(self);
            }
            else
            {
                //复制父节点的所有记录，并把这些记录的distance加一 descendant也要改成自己的
                var pnodes = _agentTreeRepository.TableNoTracking.Where(x => x.Descendant == parent).Select(x => new AgentTree
                {
                    Ancestor = x.Ancestor,
                    Descendant = user,
                    Distance = x.Distance + 1,
                });

                _agentTreeRepository.InsertMany(pnodes);
                _agentTreeRepository.Insert(self);
            }

            _agentUnitOfWork.Save();

        }


        /// <summary>
        /// 移动到parent的下级,后代也将随之移动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="parent">为空表示user作为根节点存在</param>
        public void MoveTo(string user, string parent = "")
        {
            var existed = _agentTreeRepository.TableNoTracking.Any(x => x.Descendant == user);
            if (!existed)
            {
                throw new Exception($"{user}节点已存在");
            }
            if (!string.IsNullOrEmpty(parent))
            {
                var parentexisted = _agentTreeRepository.TableNoTracking.Any(x => x.Descendant == parent);
                if (!parentexisted)
                {
                    throw new Exception($"{parent}父节点不存在");
                }
            }

            var children = _agentTreeRepository.TableNoTracking.Where(x => x.Ancestor == user)
                .Select(x => new { x.Ancestor, x.Descendant, x.Distance }).ToList();

            var pnodes = _agentTreeRepository.TableNoTracking.Where(x => x.Descendant == parent && x.Distance > 0);
            foreach (var child in children)
            {
                //user下所有子节点遍历=>删除user以上的父节点
                var safeDistance = children.FirstOrDefault(x => x.Ancestor == user && x.Descendant == child.Descendant && x.Distance > 0)?.Distance ?? 0;
                _agentTreeRepository.DeleteWhere(x => x.Descendant == child.Descendant && x.Distance > safeDistance);

                //所有user子节点遍历 =>复制parent节点的所有记录，并把这些记录的distance加一，escendant也要改成自己的
                _agentTreeRepository.InsertMany(pnodes.Select(x => new AgentTree
                {
                    Ancestor = x.Ancestor,
                    Descendant = child.Descendant,
                    Distance = x.Distance + safeDistance + 1
                }));

                if (!string.IsNullOrEmpty(parent))
                {
                    _agentTreeRepository.Insert(new AgentTree
                    {
                        Ancestor = parent,
                        Descendant = child.Descendant,
                        Distance = safeDistance + 1
                    });
                }
            }

            _agentUnitOfWork.Save();
        }

        #endregion


        #region 生成树形结构

        /// <summary>
        /// 从当前节点生成树
        /// </summary>
        public AgentTreeModel GetTree(string user, List<string> leafNodes)
        {
            AgentTreeModel root = new AgentTreeModel(user);
            //所有的叶子节点
            if (leafNodes == null || leafNodes.Count == 0)
            {
                leafNodes = _agentTreeRepository.TableNoTracking
                   .GroupBy(g => g.Ancestor).Where(x => x.Count() == 1)
                   .Select(x => x.Key).Distinct().ToList();
            }
            var tree = GetChildren(root, leafNodes);
            return tree;
        }

        /// <summary>
        /// 从根节点生成树,可能有多个
        /// </summary>
        public List<AgentTreeModel> GetRootTree()
        {
            var roots = GetRoots();
            var trees = new List<AgentTreeModel>();

            var leafNodes = _agentTreeRepository.TableNoTracking
               .GroupBy(g => g.Ancestor).Where(x => x.Count() == 1)
               .Select(x => x.Key).Distinct().ToList();

            foreach (var root in roots)
            {
                var tree = GetTree(root, leafNodes);
                trees.Add(tree);
            }

            return trees;
        }

        /// <summary>
        /// 递归向下遍历
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tree"></param>
        private AgentTreeModel GetChildren(AgentTreeModel root, List<string> leafNodes)
        {
            //查找直属下级
            var children = _agentTreeRepository.TableNoTracking
                .Where(x => x.Ancestor == root.UserName && x.Distance == 1)
                .OrderBy(x => x).Distinct()
                .Select(x => new AgentTreeModel(x.Descendant));

            root.Children = children.ToList();
            foreach (var child in root.Children)
            {
                //叶子节点跳过
                if (leafNodes.Contains(child.UserName))
                    continue;


                GetChildren(child, leafNodes);
            }

            return root;
        }

        #endregion
    }

}
