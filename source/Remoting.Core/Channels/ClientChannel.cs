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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Remoting.Core.Channels
{
	public class ClientChannel : IChannel
	{
		#region Methods

		public IRemoteService Initialize()
		{
			// set channel properties
			IDictionary props = new Hashtable();
			props["port"] = 0;
			props["name"] = "client";
			// props["timeout"] = 5000;

			BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider();
			sinkProvider.TypeFilterLevel = TypeFilterLevel.Full;

			// create and register the channel
			TcpChannel clientChannel = new TcpChannel(props,
				new BinaryClientFormatterSinkProvider(), sinkProvider);
			ChannelServices.RegisterChannel(clientChannel, false);

			// create transparent proxy to server component
			return (IRemoteService)Activator.GetObject(
				typeof(IRemoteService), "tcp://localhost:9000/service.rem");
		}

		#endregion Methods
	}
}