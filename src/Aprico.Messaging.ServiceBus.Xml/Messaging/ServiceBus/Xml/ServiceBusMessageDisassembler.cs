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
/// Disassembles <see cref="ServiceBusReceivedMessage"/> messages and provides XML deserialization of their
/// <see cref="ServiceBusReceivedMessage.Body"/>.
/// </summary>
/// <remarks>This class enables message processing of XML-serialized <see cref="ServiceBusReceivedMessage"/> message payloads.</remarks>
[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
public class ServiceBusMessageDisassembler(MessageContractRegistry messageContractRegistry) : IMessageDisassembler<ServiceBusReceivedMessage>
{
	#region IMessageDisassembler<ServiceBusReceivedMessage> Members

	/// <summary>
	/// Disassembles a <see cref="ServiceBusReceivedMessage"/> and returns its XML-deserialized
	/// <see cref="ServiceBusReceivedMessage.Body"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> message whose <see cref="ServiceBusReceivedMessage.Body"/> is
	/// to be deserialized.
	/// </param>
	/// <returns>The deserialized payload object contained in the <see cref="ServiceBusReceivedMessage.Body"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input <paramref name="message"/> is <see langword="null"/>.</exception>
	public object DeserializeBody(ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		var contractType = _messageContractRegistry.GetRegisteredContract(message.GetMessageBodyType());
		return XmlBodyDeserializer.Deserialize(contractType, message.Body);
	}

	#endregion

	private readonly MessageContractRegistry _messageContractRegistry = messageContractRegistry ?? throw new ArgumentNullException(nameof(messageContractRegistry));
}
