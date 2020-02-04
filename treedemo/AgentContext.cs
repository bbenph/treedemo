using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    //web根目录(Program.cs同级目录) 执行 dotnet ef migrations add InitialCreate
    // 再执行 dotnet ef database update
    // 自己生成数据库
    public class AgentContext: DbContext
    {
        public AgentContext()
        {

        }
        public AgentContext(DbContextOptions<AgentContext> options)
        : base(options)
        {

        }

        public DbSet<AgentTree> AgentTrees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AgentTreeConfiguration());
        }
    }
}
