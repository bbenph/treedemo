using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    public interface IAgentRelationshipService
    {
        /// <summary>
        /// 创建一个新的节点
        /// </summary>
        /// <param name="user"></param>
        /// <param name="parent">默认空,表示创建根节点,否则 创建到parent节点下</param>
        void Create(string user, string parent = "");
        /// <summary>
        /// 将此节点作为根(如有后代也将随之移动),等同于 MoveTo(user,"");
        /// </summary>
        /// <param name="user"></param>
        void MakeRoot(string user);
        /// <summary>
        /// 移动到parent的下级,后代也将随之移动
        /// </summary>
        /// <param name="user"></param>
        /// <param name="to">空表示user作为根节点存在</param>
        void MoveTo(string user, string to = "");
        /// <summary>
        /// 获取所有后代
        /// </summary>
        /// <param name="user"></param>
        /// <param name="includeSelf">是否包含自己</param>
        /// <returns></returns>
        List<string> GetDescendants(string user, bool includeSelf = false);
        /// <summary>
        /// 获取所有祖先
        /// </summary>
        /// <param name="user"></param>
        /// <param name="includeSelf">是否包含自己</param>
        /// <returns></returns>
        List<string> GetAncestors(string user, bool includeSelf = false);
        /// <summary>
        /// 获取直接下级
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<string> GetChildren(string user);
        /// <summary>
        /// 获取直接上级
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetParent(string user);
        /// <summary>
        /// 获取祖先(根)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRoot(string user);
        /// <summary>
        /// 获取所有兄弟姐妹(同一个parent)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        List<string> GetSiblings(string user, bool includeSelf = false);
        /// <summary>
        /// 获取所有根
        /// </summary>
        /// <returns></returns>
        List<string> GetRoots();
        /// <summary>
        /// 从根节点生成树,可能有多个
        /// </summary>
        /// <returns></returns>
        List<AgentTreeModel> GetRootTree();
        /// <summary>
        /// 从当前节点生成树
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        AgentTreeModel GetTree(string user, List<string> leafNodes = null);
    }

}
