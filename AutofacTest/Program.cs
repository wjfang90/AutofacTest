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
            var p = container.Resolve<IChineseDal>();
            p.SayHello("张三 p");


            var p2 = container.ResolveNamed<IChineseDal>("Chinese");
            p2.SayHello("张三");



            using (var scope = container.BeginLifetimeScope())
            {
                var chlDal4 = scope.ResolveNamed("Chl", typeof(IChineseDal));
            }



            Console.ReadKey();
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
