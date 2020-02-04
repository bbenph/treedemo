using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treedemo
{
    public class AgentTree
    {
        public int Id { get; set; }
        /// <summary>
        /// 祖先：上级节点username
        /// </summary>
        public string Ancestor { get; set; }
        /// <summary>
        /// 子代：下级节点username
        /// </summary>
        public string Descendant { get; set; }
        /// <summary>
        /// 距离：子代到祖先中间隔了几级
        /// </summary>
        public int Distance { get; set; }
    }

    public class AgentTreeConfiguration : IEntityTypeConfiguration<AgentTree>
    {
        public void Configure(EntityTypeBuilder<AgentTree> builder)
        {
            builder.ToTable("AAAAATest_AgentTree");

            builder.Property(x => x.Ancestor).IsRequired().HasDefaultValue(string.Empty);
            builder.Property(x => x.Descendant).IsRequired().HasDefaultValue(string.Empty);
            builder.Property(x => x.Distance).IsRequired();

            builder.HasIndex(x => new { x.Ancestor, x.Descendant, x.Distance }).IsUnique();
        }
    }
}
