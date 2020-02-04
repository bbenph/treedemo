using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    public class AgentTreeModel
    {
        public AgentTreeModel(string userName)
        {
            this.UserName = userName;
        }
        public string UserName { get; set; }
        public ICollection<AgentTreeModel> Children { get; set; }
    }
}
