using System;
using System.Reflection;
using System.Diagnostics;

using ECommon.Autofac;
using ECommon.Components;
using ECommon.Configurations;
using ECommon.Extensions;
using ECommon.IO;
using ECommon.JsonNet;
using ECommon.Log4Net;
using ECommon.Logging;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Configurations;

using Console.Extensions;
using Commands.Grab;
using Commands.OrderProcess;

namespace Console
{
    public class Program
    {
        private static ILogger _logger;

        static void Main(string[] args)
        {
            InitializeENodeFramework();

            var _commandService = ObjectContainer.Resolve<ICommandService>();

            #region 测试
            string orderId = "ENODE_TEST_10705";
            long operSoufunId = 914154500060;
            string operSoufunName = "ENode_Test_Data_Admin_60";
            long ownerSoufunId = 514154500705;
            int count = 10705;
            Stopwatch timer = new Stopwatch();
            for (int i = 0; i < 100; i++)
            {
                timer.Restart();
                var command = new GrabOrderCommand(ObjectId.GenerateNewStringId(), orderId, operSoufunId, operSoufunName, ownerSoufunId);
                var result = _commandService.ExecuteAsync(command, CommandReturnType.EventHandled).WaitResult<AsyncTaskResult<CommandResult>>(5000);
                //var result = commandService.SendAsync(command);
                timer.Stop();
                System.Console.WriteLine("当前Command通过ExecuteAsync方法执行成功的时间为：[{0} ms], OrderId为： [{1}]", timer.ElapsedMilliseconds, orderId);
                count++;
                orderId = "ENODE_TEST_" + count;
                ownerSoufunId++;
                System.Console.ReadKey();
            }
            #endregion

            System.Console.ReadKey();
        }

        static void InitializeENodeFramework()
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            Configuration
                .Create()
                .UseAutofac()
                .RegisterCommonComponents()
                .UseLog4Net()
                .UseJsonNet()
                .RegisterUnhandledExceptionHandler()
                .CreateENode()
                .RegisterENodeComponents()
                .RegisterBusinessComponents(assemblies)
                .RegisterAllTypeCodes()
                .UseEQueue()
                .InitializeBusinessAssemblies(assemblies)
                .StartEQueue();

            _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(Program).Name);
            _logger.Info("Command Producer started.");
        }
    }
}
