#region Copyright & License

// Copyright © 2024 - 2025 Aprico Consultants
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using Aprico.Messaging.Abstractions;
using Aprico.Messaging.Message.Deserializer;
using Aprico.Messaging.ServiceBus.Extensions;
using Azure.Messaging.ServiceBus;

namespace Aprico.Messaging.ServiceBus.Xml;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API.")]
public class XmlMessageDeserializer : AbstractXmlMessageDeserializer<XmlMessageDeserializer>, IMessageDeserializer<ServiceBusReceivedMessage>
{
	#region IMessageDeserializer<ServiceBusReceivedMessage> Members

	public object DeserializeBody(ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		var contractType = GetXmlContract(message.GetMessageBodyType());
		return DeserializeBody(contractType, message.Body);
	}

	#endregion
}
