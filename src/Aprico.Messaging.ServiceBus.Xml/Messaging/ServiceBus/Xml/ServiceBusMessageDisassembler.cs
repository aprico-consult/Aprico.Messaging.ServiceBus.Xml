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
using Aprico.Messaging.Contracts.Deserialization;
using Aprico.Messaging.Message;
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
public class ServiceBusMessageDisassembler : IMessageDisassembler<ServiceBusReceivedMessage>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ServiceBusMessageDisassembler"/> class with an optional message contract
	/// registry.
	/// </summary>
	/// <param name="messageContractRegistry">
	/// The message contract registry to be used for message type identification and
	/// deserialization. Can be <see langword="null"/> if no specific registry is required.
	/// </param>
	/// <remarks>
	/// This constructor allows you to provide a custom <see cref="IMessageContractRegistry"/> for managing message contract
	/// types during deserialization.
	/// </remarks>
	public ServiceBusMessageDisassembler(IMessageContractRegistry? messageContractRegistry = null)
	{
		_messageContractRegistry = messageContractRegistry;
	}

	#region IMessageDisassembler<ServiceBusReceivedMessage> Members

	/// <summary>
	/// Deserializes a <see cref="ServiceBusReceivedMessage"/> and returns its XML-deserialized
	/// <see cref="ServiceBusReceivedMessage.Body"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> message whose <see cref="ServiceBusReceivedMessage.Body"/> is
	/// to be deserialized.
	/// </param>
	/// <returns>The deserialized payload object contained in the <see cref="ServiceBusReceivedMessage.Body"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input <paramref name="message"/> is <see langword="null"/>.</exception>
	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed", Justification = "Validated by callee.")]
	public object DeserializeBody(ServiceBusReceivedMessage message)
	{
		return DeserializeBody(message, _messageContractRegistry!);
	}

	/// <summary>
	/// Deserializes a <see cref="ServiceBusReceivedMessage"/> using the provided <see cref="IMessageContractRegistry"/> and
	/// returns its XML-deserialized <see cref="ServiceBusReceivedMessage.Body"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> message whose <see cref="ServiceBusReceivedMessage.Body"/> is
	/// to be deserialized.
	/// </param>
	/// <param name="messageContractRegistry">The registry containing message contract information.</param>
	/// <returns>The deserialized object representing the message body.</returns>
	/// <exception cref="ArgumentNullException">
	/// Thrown when either the <paramref name="message"/> or
	/// <paramref name="messageContractRegistry"/> is null.
	/// </exception>
	public object DeserializeBody(ServiceBusReceivedMessage message, IMessageContractRegistry messageContractRegistry)
	{
		ArgumentNullException.ThrowIfNull(message);
		ArgumentNullException.ThrowIfNull(messageContractRegistry);
		var contractType = messageContractRegistry.GetRegisteredContract(message.GetMessageBodyType());
		return XmlBodyDeserializer.Deserialize(contractType, message.Body);
	}

	#endregion

	private readonly IMessageContractRegistry? _messageContractRegistry;
}
