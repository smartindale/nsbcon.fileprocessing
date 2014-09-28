
namespace Acme.FileManagement.New.Endpoint
{
    using NServiceBus;
    using System;
    using System.Configuration;

	/*
		This class configures this endpoint as a Server. More information about how to configure the NServiceBus host
		can be found here: http://particular.net/articles/the-nservicebus-host
	*/
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        static string _databusShare = ConfigurationManager.AppSettings["FileShareDataBus"] ?? @"C:\databus";
        public void Init()
        {
            Configure.With()
                 .StructureMapBuilder()
                 .UseTransport<Msmq>()
                 .FileShareDataBus(_databusShare)
                 .PurgeOnStartup(System.Diagnostics.Debugger.IsAttached)
                 .DefiningDataBusPropertiesAs(pi => pi.Name.EndsWith("Attachment") || pi.Name.EndsWith("CudlConnectMessage") || pi.Name.EndsWith("RawConfiguration"))
                 .DefiningCommandsAs(pi => pi.Namespace != null && pi.Namespace.Contains(".Contracts.Commands"))
                 .DefiningEventsAs(pi => pi.Namespace != null && pi.Namespace.Contains(".Contracts.Events"))
                 .DefiningMessagesAs(pi => pi.Namespace != null && pi.Namespace.Contains(".Contracts.Messages"))
                 .DefiningExpressMessagesAs(pi => pi.Name.EndsWith("Express"))
                 .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires") ? TimeSpan.FromSeconds(45) : TimeSpan.MaxValue);


        }
    }
}
