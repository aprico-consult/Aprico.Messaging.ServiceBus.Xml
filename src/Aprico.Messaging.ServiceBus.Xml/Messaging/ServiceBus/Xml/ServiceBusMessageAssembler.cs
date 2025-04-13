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
using Aprico.Extensions;
using Aprico.Messaging.Message;
using Aprico.Xml.Extensions;
using Azure.Messaging.ServiceBus;
using Be.Stateless.Extensions;

namespace Aprico.Messaging.ServiceBus.Xml;

/// <summary>Provides XML message serialization functionality for <see cref="ServiceBusMessage"/>s.</summary>
/// <remarks>
/// This class implements message serialization for XML-based messages in a Service Bus context, supporting custom context
/// properties and configurations.
/// </remarks>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API.")]
public class ServiceBusMessageAssembler : IMessageAssembler<ServiceBusMessage>
{
	#region IMessageSerializer<ServiceBusMessage> Members

	/// <summary>Serializes a body object into a <see cref="ServiceBusMessage"/> with optional context properties.</summary>
	/// <typeparam name="TBody">The XML contract type of the message body, which must be a non-null type.</typeparam>
	/// <param name="body">The body object to be serialized.</param>
	/// <param name="messageId">Optional unique identifier for the message. Generated if not provided.</param>
	/// <param name="correlationId">Optional correlation identifier for the message.</param>
	/// <param name="sessionId">Optional session identifier for the message.</param>
	/// <param name="businessId">Optional <see cref="ApplicationPropertyNames.BusinessId"/> for the message.</param>
	/// <param name="timestamp">
	/// Optional <see cref="ApplicationPropertyNames.Timestamp"/> for the message. Uses current UTC time if not
	/// provided.
	/// </param>
	/// <param name="scheduledEnqueueTime">Optional scheduled enqueue time for the message. Uses current UTC time if not provided.</param>
	/// <returns>
	/// A <see cref="ServiceBusMessage"/> whose <see cref="ServiceBusMessage.Body"/> contains the serialized representation of
	/// the given <paramref name="body"/> argument.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="body"/> is <see langword="null"/>.</exception>
	public ServiceBusMessage Assemble<TBody>(
		TBody body,
		string? messageId = null,
		string? correlationId = null,
		string? sessionId = null,
		string? businessId = null,
		DateTimeOffset? timestamp = null,
		DateTimeOffset? scheduledEnqueueTime = null)
		where TBody : notnull
	{
		ArgumentNullException.ThrowIfNull(body);

		// @formatter:wrap_chained_method_calls chop_if_long
		var message = new ServiceBusMessage(body.SerializeAsXmlBinary()) {
			MessageId = messageId ?? Guid.NewGuid().ToString("D"),
			CorrelationId = correlationId,
			ReplyToSessionId = sessionId,
			ScheduledEnqueueTime = scheduledEnqueueTime ?? DateTimeOffset.UtcNow
		};
		message.ApplicationProperties.Add(ApplicationPropertyNames.MessageBodyType, body.GetXmlFullyQualifiedName());
		message.ApplicationProperties.Add(ApplicationPropertyNames.Timestamp, (timestamp ?? DateTimeOffset.UtcNow).ToString("o"));
		businessId.IfNotNullOrEmpty(bid => message.ApplicationProperties.Add(ApplicationPropertyNames.BusinessId, bid));
		return message;
	}

	#endregion
}
