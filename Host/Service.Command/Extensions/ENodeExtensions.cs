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
using EQueue.Configurations;

using Domain.Grab;
using Domain.OrderProcess;
using Commands.Grab;
using Commands.OrderProcess;

namespace Service.Command.Extensions
{
    public static class ENodeExtensions
    {
        private static CommandConsumer _commandConsumer;
        private static DomainEventPublisher _domainEventPublisher;

        public static ENodeConfiguration UseEQueue(this ENodeConfiguration enodeConfiguration)
        {
            var configuration = enodeConfiguration.GetCommonConfiguration();

            configuration.RegisterEQueueComponents();

            _domainEventPublisher = new DomainEventPublisher();

            configuration.SetDefault<IMessagePublisher<DomainEventStreamMessage>, DomainEventPublisher>(_domainEventPublisher);

            _commandConsumer = new CommandConsumer();

            _commandConsumer
                .Subscribe("GrabOrderCommandTopic")
                .Subscribe("ProcessingOrderCommandTopic");

            return enodeConfiguration;
        }

        public static ENodeConfiguration StartEQueue(this ENodeConfiguration enodeConfiguration)
        {
            _commandConsumer.Start();
            _domainEventPublisher.Start();
            WaitAllConsumerLoadBalanceComplete();

            return enodeConfiguration;
        }

        public static ENodeConfiguration RegisterAllTypeCodes(this ENodeConfiguration enodeConfiguration)
        {
            var provider = ObjectContainer.Resolve<ITypeCodeProvider>() as DefaultTypeCodeProvider;

            //aggregates
            provider.RegisterType<GrabOrder>(40);
            provider.RegisterType<ProcessingOrder>(50);

            //commands
            provider.RegisterType<GrabOrderCommand>(400000);
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
                var commandConsumerAllocatedQueues = _commandConsumer.Consumer.GetCurrentQueues();
                if (commandConsumerAllocatedQueues.Count() == 8)
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
