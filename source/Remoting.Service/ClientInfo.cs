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
using System.Collections.Generic;
using System.Text;

namespace Remoting.Service
{
	[Serializable]
	public class ClientInfo
	{
		#region Fields

		private string clientId;
		private delCommsInfo hostToClient;

		#endregion Fields

		#region Constructors

		public ClientInfo(string clientId, delCommsInfo hostToClient)
		{
			this.clientId = clientId;
			this.hostToClient = hostToClient;
		}

		#endregion Constructors

		#region Delegates

		public delegate void ClientInfoCallback(object o);

		#endregion Delegates

		#region Events

		public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		#endregion Events

		#region Properties

		public string ClientId
		{
			get
			{
				return clientId;
			}
		}

		public delCommsInfo HostToClient
		{
			get { return hostToClient; }
		}

		#endregion Properties

		#region Methods

		/* public override bool Equals(Object obj)
		{
			if (obj == null)
			{
				return false;
			}

			ClientInfo other = obj as ClientInfo;
			if ((Object)other == null)
			{
				return false;
			}

			return (clientId == other.ClientId);
		} */
		/* public bool Equals(ClientInfo other)
		{
			// If parameter is null return false:
			if ((object)other == null)
			{
				return false;
			}

			// Return true if the fields match:
			return (clientId == other.ClientId);
		} */
		/* public override int GetHashCode()
		{
			return clientId.GetHashCode();
		} */
		public void Send(string clientId, Object obj)
		{
			HostToClient("Hello World");
			// OnMessageReceived(new MessageReceivedEventArgs(clientId, obj));
		}

		private void OnMessageReceived(MessageReceivedEventArgs e)
		{
			if (MessageReceived != null)
			{
				MessageReceived(this, e);
			}
		}

		#endregion Methods
	}
}