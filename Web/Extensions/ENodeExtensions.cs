using System.Net;

using ECommon.Components;
using ECommon.Utilities;
using ENode.Commanding;
using ENode.Configurations;
using ENode.EQueue;
using ENode.EQueue.Commanding;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using EQueue.Configurations;

using Commands.Grab;
using Commands.OrderProcess;

namespace Web.Extensions
{
    public static class ENodeExtensions
    {
        private static CommandService _commandService;
        private static CommandResultProcessor _commandResultProcessor;

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            provider.RegisterType<GrabOrderCommand>(11200);
            provider.RegisterType<ProcessingOrderCommand>(12201);

            return enodeConfiguration;
        }
        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            var bindingIP = System.Configuration.ConfigurationManager.AppSettings["ProducerIP"];
            var ProducerEndPoint = string.IsNullOrEmpty(bindingIP) ? SocketUtils.GetLocalIPV4() : IPAddress.Parse(bindingIP);
            _commandResultProcessor = new CommandResultProcessor(new IPEndPoint(ProducerEndPoint, 9022));
            _commandService = new CommandService(_commandResultProcessor);

            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            return enodeConfiguration;
        }
        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandService.Start();
            return enodeConfiguration;
        }
    }
}