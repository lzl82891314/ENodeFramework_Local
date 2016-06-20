using System;

using ECommon.Components;
using ECommon.Configurations;
using ECommon.Logging;
using ECommon.Autofac;
using ECommon.Log4Net;
using ECommon.JsonNet;
using EQueue.Broker;
using EQueue.Configurations;

using Infrastructure;

namespace Service.Broker
{
    class Program
    {
        private static Configuration _ecommonConfiguration;

        static void Main(string[] args)
        {
            InitializeEQueue();
            BrokerController.Create().Start();

            ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program).Name).Info("Broker started, press Enter to exit...");
            Console.ReadLine();
        }

        static void InitializeEQueue()
        {
            _ecommonConfiguration = Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterEQueueComponents();

            ConfigSettings.Initialize();
            var queueStoreSetting = new SqlServerQueueStoreSetting
            {
                ConnectionString = ConfigSettings.ConnectionString
            };
            var messageStoreSetting = new SqlServerMessageStoreSetting
            {
                ConnectionString = ConfigSettings.ConnectionString,
                MessageLogFilePath = "/equeue_message_logs"
                //PersistMessageInterval = 1000,
                //PersistMessageMaxCount = 5000,
                ////MessageLogFile = "log/log.log",
                //MessageMaxCacheSize = 200000
            };
            var offsetManagerSetting = new SqlServerOffsetManagerSetting
            {
                ConnectionString = ConfigSettings.ConnectionString
            };

            _ecommonConfiguration
                .RegisterEQueueComponents()
                .UseSqlServerQueueStore(queueStoreSetting)
                .UseSqlServerMessageStore(messageStoreSetting)
                .UseSqlServerOffsetManager(offsetManagerSetting);
        }
    }
}
