using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using treedemo;

namespace NUnitTestProject1
{
    public class DI
    {
        public Autofac.IContainer DICollections()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<AgentContext>(options =>
                options.UseMySql(AppConfigurtaionService.Configuration.GetConnectionString("Agent")));

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            services.AddScoped(typeof(IRepository<>), typeof(AgentRepository<>));
            services.AddScoped(typeof(IAgentRelationshipService), typeof(AgentRelationshipService));


            //实例化 AutoFac  容器   
            var builder = new ContainerBuilder();
            //将services填充到Autofac容器生成器中
            builder.Populate(services);

            //使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            return ApplicationContainer;
        }
    }
}
