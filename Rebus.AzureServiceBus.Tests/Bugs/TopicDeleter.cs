﻿using System;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Rebus.Internals;

namespace Rebus.AzureServiceBus.Tests.Bugs
{
    public class TopicDeleter : IDisposable
    {
        readonly string _topicName;

        public TopicDeleter(string topicName)
        {
            _topicName = topicName;
        }

        public void Dispose()
        {
            var managementClient = new ManagementClient(AsbTestConfig.ConnectionString);

            AsyncHelpers.RunSync(async () =>
            {
                try
                {
                    var topicDescription = await managementClient.GetTopicAsync(_topicName);

                    await managementClient.DeleteTopicAsync(topicDescription.Path);

                    Console.WriteLine($"Deleted topic '{_topicName}'");
                }
                catch (MessagingEntityNotFoundException)
                {
                    // it's ok
                }
            });
        }
    }
}