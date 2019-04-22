using Autofac;
using Autofac.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace AutofacTest
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            RegisterByConfigFile();

            //RgisterAndResolveNamed();

            Console.ReadKey();
        }

        private static void RegisterByConfigFile()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");

            var module = new ConfigurationModule(config.Build());

            var builder = new ContainerBuilder();

            //class StandardConsole : IStartable, IConsumer<ConsoleCommand>
            //builder.RegisterType<StandardConsole>()
            //       .As<IStartable>()
            //       .As<IConsumer<ConsoleCommand>>()
            //       .SingleInstance();

            builder.RegisterModule(module);
            //builder.populate(services);

            //return new AutofacServiceProvider(builder.Build());

            var container = builder.Build();

            //var p = container.Resolve<IPerson>();
            //var p = container.Resolve<IPerson<Animal>>();

            //json file 不能配置service.key
            //var p = container.Resolve<IChineseDal>();
            //p.SayHello("张三 p");

            //json file 配置service.key="Chinese"
            var p1 = container.ResolveKeyed<IChineseDal>("Chinese");
            p1.SayHello("张三");


            //to do  json file 配置解析不出来，原因未找到
            //var p2 = container.ResolveNamed<IChineseDal>("Chinese");
            //p2.SayHello("张三");



            //using (var scope = container.BeginLifetimeScope())
            //{
            //    //var dal = scope.ResolveNamed("Chinese", typeof(IChineseDal));
            //    var dal = (scope.ResolveKeyed("Chinese", typeof(IChineseDal))) as IChineseDal;
            //    dal.SayHello("李四");
            //}
        }

        private static void RgisterAndResolveNamed()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ChineseDal>().Named<IChineseDal>("Chinese");

            var container = containerBuilder.Build();

           var dal= container.ResolveNamed<IChineseDal>("Chinese");
            dal.SayHello("张三 ResolveNamed");

        }
    }



    public class ChineseDal : BaseDal<Chinese>, IChineseDal
    {
        public void SayHello(string name)
        {
            Console.WriteLine($"{name} 对您说，你好！");
        }
    }

    public class BaseDal<T> where T : Person, new()
    {

    }

    public interface IDal<T> where T : Person, new()
    {
        void SayHello(string name);
    }

    public interface IChineseDal : IDal<Chinese>
    {

    }

    public class Person
    {

    }

    public class Chinese : Person
    {

    }




}
