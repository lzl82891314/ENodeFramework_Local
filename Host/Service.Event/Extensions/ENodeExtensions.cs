using System;
using System.Threading;
using System.Linq;

using ECommon.Components;
using ECommon.Logging;
using ECommon.Scheduling;
using ENode.Configurations;
using ENode.EQueue;
using ENode.Eventing;
using ENode.Infrastructure;
using ENode.Infrastructure.Impl;
using ENode.Commanding;
using EQueue.Configurations;

using Domain.Grab;
using Domain.OrderProcess;
using Commands.Grab;
using Commands.OrderProcess;
using Denormalizers.Dapper.Grab;
using Denormalizers.Dapper.OrderProcess;
using ProcessManagers;

namespace Service.Event.Extensions
{
    public static class ENodeExtensions
    {
        private static DomainEventConsumer _eventConsumer;
        private static CommandService _commandService;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _commandService = new CommandService(id: "CommandServiceForProcessManager");
            configuration.SetDefault<ICommandService, CommandService>(_commandService);

            _eventConsumer = new DomainEventConsumer();

            _eventConsumer
                .Subscribe("GrabOrderEventTopic")
                .Subscribe("ProcessingOrderEventTopic");

            return enodeConfiguration;
        }

        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _eventConsumer.Start();
            _commandService.Start();
            WaitAllConsumerLoadBalanceComplete();

            return enodeConfiguration;
        }

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //commands
            provider.RegisterType<ProcessingOrderCommand>(500000);

            //events
            provider.RegisterType<QiangLockOrderCreateEvent>(401000);
            provider.RegisterType<WaitQiangUpdateEvent>(401001);

            provider.RegisterType<AttentionOrderUpdateEvent>(501001);
            provider.RegisterType<NCustomerListUpdateEvent>(501002);
            provider.RegisterType<NOrderBaseUpdateEvent>(501003);
            provider.RegisterType<NOrderServiceCreateEvent>(501004);
            provider.RegisterType<NOrderServiceLogCreateEvent>(501005);
            provider.RegisterType<NOrderServiceLogUpdateEvent>(501006);
            provider.RegisterType<NOrderServiceUpdateEvent>(501007);
            provider.RegisterType<OrderFollowUpCreateEvent>(501008);
            provider.RegisterType<RealSiteInfoUpdateEvent>(501010);
            provider.RegisterType<OrderFollowUpCreateSuccessEvent>(501011);

            //denormalizers
            provider.RegisterType<GrabOrderDenormalizer>(402000);
            provider.RegisterType<ProcessingOrderDenormalizer>(502000);

            //process manager
            provider.RegisterType<GrabOrderProcessManager>(1000);

            return enodeConfiguration;
        }

        private static void WaitAllConsumerLoadBalanceComplete()
        {
            var logger = ObjectContainer.Resolve<ILoggerFactory>().Create(typeof(ENodeExtensions).Name);
            var scheduleService = ObjectContainer.Resolve<IScheduleService>();
            var waitHandle = new ManualResetEvent(false);
            logger.Info("Waiting for all consumer load balance complete, please wait for a moment...");
            var taskId = scheduleService.ScheduleTask("WaitAllConsumerLoadBalanceComplete", () =>
            {
                var eventConsumerAllocatedQueues = _eventConsumer.Consumer.GetCurrentQueues();
                if (eventConsumerAllocatedQueues.Count() == 8)
                {
                    waitHandle.Set();
                }
            }, 1000, 1000);

            waitHandle.WaitOne();
            scheduleService.ShutdownTask(taskId);
            logger.Info("All consumer load balance completed.");
        }
    }
}
