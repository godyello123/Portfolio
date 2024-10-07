#define USE_CONFIG

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SCommon
{
	public class SWCFServer<IService, Service> : IDisposable
	{
		private bool m_Disposed;

		private ServiceHost m_Host;

		~SWCFServer()
		{
			Dispose(false);
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if(m_Disposed) return;
			if(disposing)
			{
				if(m_Host != null) m_Host.Close();
			}
			m_Disposed = true;
		}

		public void Init()
		{
			m_Host = new ServiceHost(typeof(Service));
			m_Host.Open();
		}

		public void Init(string uri, Binding binding)
		{
			m_Host = new ServiceHost(typeof(Service), new Uri(uri));
			m_Host.AddServiceEndpoint(typeof(IService), binding, "");
			m_Host.Open();
		}
	}

	public class SWCFClient<IService> : IDisposable
	{
		private bool m_Disposed;

		public IService Proxy { get; set; }

		~SWCFClient()
		{
			Dispose(false);
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if(m_Disposed) return;
			if(disposing)
			{
				if(Proxy != null) ((IDisposable)Proxy).Dispose();
			}
			m_Disposed = true;
		}

		public void InitWithServiceName(string serviceName)
		{
			ChannelFactory<IService> factory = new ChannelFactory<IService>(serviceName);
			Proxy = factory.CreateChannel();
		}
		public void Init(string uri, Binding binding)
		{
			ServiceEndpoint endPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(IService)), binding, new EndpointAddress(new Uri(uri)));
			ChannelFactory<IService> factory = new ChannelFactory<IService>(endPoint);
			Proxy = factory.CreateChannel();
		}
	}
}
