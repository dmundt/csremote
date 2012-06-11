#region Header

// Copyright (C) 2012 Daniel Schubert
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Remoting.Service;
using Remoting.Service.Events;

namespace Remoting.Client
{
	public partial class FormMain : Form
	{
		#region Fields

		private IRemoteService remoteService;

		#endregion Fields

		#region Constructors

		public FormMain()
		{
			InitializeComponent();
			InitializeClient();
		}

		#endregion Constructors

		#region Delegates

		delegate void SetTextCallback(string text);

		#endregion Delegates

		#region Methods

		public void SendMessage(string proxyId)
		{
			try
			{
				if (remoteService == null)
				{
					// create transparent proxy to server component
					remoteService = (IRemoteService)Activator.GetObject(
						typeof(IRemoteService), "tcp://localhost:9001/service.rem");
				}
				EventProxy eventProxy = new EventProxy(proxyId);
				eventProxy.EventDispatched += new EventHandler<EventDispatchedEventArgs>(eventProxy_EventDispatched);
				remoteService.DispatchCall(eventProxy, "Hello World");
			}
			catch (RemotingException ex)
			{
				MessageBox.Show(this, ex.Message, "Error");
			}
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			SendMessage(tbClientId.Text);
		}

		private void eventProxy_EventDispatched(object sender, EventDispatchedEventArgs e)
		{
			SetText(string.Format("EventDispatched: {0}{1}",
				(string)e.Data, Environment.NewLine));
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			tbClientId.Text = Guid.NewGuid().ToString("N");
		}

		private void InitializeClient()
		{
			// set channel properties
			IDictionary props = new Hashtable();
			props["port"] = 0;
			props["name"] = "client";

			BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider();
			sinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

			// create and register the channel
			TcpChannel clientChannel = new TcpChannel(props,
				new BinaryClientFormatterSinkProvider(), sinkProvider);
			ChannelServices.RegisterChannel(clientChannel, false);
		}

		private void SetText(string text)
		{
			// thread-safe call
			if (tbLog.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				tbLog.AppendText(text);
			}
		}

		#endregion Methods
	}
}