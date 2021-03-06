﻿namespace NServiceBus.Features
{
    using AmazonSQS;
    using Transports;
    using Transports.SQS;

    public class SqsTransportFeature : ConfigureTransport
    {
        protected override void Configure(FeatureConfigurationContext context, string connectionString)
        {
            var connectionConfiguration = SqsConnectionStringParser.Parse(connectionString);

			context.Container.ConfigureComponent(_ => connectionConfiguration, DependencyLifecycle.SingleInstance);

            context.Container.ConfigureComponent(_ => AwsClientFactory.CreateSqsClient(connectionConfiguration), DependencyLifecycle.SingleInstance);

            context.Container.ConfigureComponent(_ => AwsClientFactory.CreateS3Client(connectionConfiguration), DependencyLifecycle.SingleInstance);

			context.Container.ConfigureComponent<SqsQueueUrlCache>(DependencyLifecycle.SingleInstance);

			context.Container.ConfigureComponent<SqsDequeueStrategy>(DependencyLifecycle.InstancePerCall);
				
			context.Container.ConfigureComponent<SqsQueueSender>(DependencyLifecycle.InstancePerCall);

			context.Container.ConfigureComponent<SqsQueueCreator>(DependencyLifecycle.InstancePerCall);

            if ( context.Settings.UseSqsDeferral() )
                context.Container.ConfigureComponent<SqsDeferrer>(DependencyLifecycle.InstancePerCall);
        }

        protected override string ExampleConnectionStringForErrorMessage
        {
			get { return "Region=ap-southeast-2;S3BucketForLargeMessages=myBucketName;S3KeyPrefix=my/key/prefix;"; }
        }
	}
}
