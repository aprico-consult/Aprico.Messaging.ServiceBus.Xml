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

/// <summary>
/// Provides XML deserialization functionality for <see cref="ServiceBusReceivedMessage"/>'s
/// <see cref="ServiceBusReceivedMessage.Body"/>.
/// </summary>
/// <remarks>
/// This class implements message deserialization for XML-based messages in a Service Bus context. It inherits from
/// <see cref="AbstractXmlMessageDeserializer{XmlMessageDeserializer}"/> and implements the
/// <see cref="IMessageDeserializer{ServiceBusReceivedMessage}"/> interface.
/// </remarks>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API.")]
public class XmlMessageDeserializer : AbstractXmlMessageDeserializer<XmlMessageDeserializer>, IMessageDeserializer<ServiceBusReceivedMessage>
{
	#region IMessageDeserializer<ServiceBusReceivedMessage> Members

	/// <summary>Deserializes the body of a <see cref="ServiceBusReceivedMessage"/> to its corresponding object type.</summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> message whose <see cref="ServiceBusReceivedMessage.Body"/> is
	/// to be deserialized.
	/// </param>
	/// <returns>
	/// The deserialized object representing the
	/// <see cref="ServiceBusReceivedMessage.Body">ServiceBusReceivedMessage.Body</see>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public object DeserializeBody(ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		var contractType = GetXmlContract(message.GetMessageBodyType());
		return DeserializeBody(contractType, message.Body);
	}

	#endregion
}
