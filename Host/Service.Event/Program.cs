using System;
using System.Reflection;

using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Autofac;
using ECommon.Log4Net;
using ECommon.JsonNet;
using EQueue.Broker;
using EQueue.Configurations;
using ENode.Configurations;

using Infrastructure;
using Service.Event.Extensions;

namespace Service.Event
{
    class Program
    {
        private static ILogger _logger;
        private static Configuration _ecommonConfiguration;
        private static ENodeConfiguration _enodeConfiguration;

        static void Main(string[] args)
        {
            InitializeENodeFramework();
            Console.ReadLine();
        }

        static void InitializeENodeFramework()
        {
            _ecommonConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler();
            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program).FullName);
            _logger.Info("ECommon initialized.");

            ConfigSettings.Initialize();

            var assemblies = new[] { 
                
                Assembly.Load("Domain"),              
                Assembly.Load("Infrastructure"),
                Assembly.Load("Denormalizers.Dapper"),
                Assembly.Load("ProcessManagers"),
                Assembly.GetExecutingAssembly()
            };

            var setting = new ConfigurationSetting
            {
                SqlDefaultConnectionString = ConfigSettings.ConnectionString
            };

            _enodeConfiguration = _ecommonConfiguration
                .CreateENode(setting)
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .RegisterAllTypeCodes()
                .UseSqlServerSequenceMessagePublishedVersionStore()
                .UseSqlServerMessageHandleRecordStore()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();
            _logger.Info("ENode initialized.");

            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program).Name).Info("Event Processor started.");
        }
    }
}
