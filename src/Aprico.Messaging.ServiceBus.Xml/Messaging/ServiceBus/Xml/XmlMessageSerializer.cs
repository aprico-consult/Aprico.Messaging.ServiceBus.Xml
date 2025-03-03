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
using Aprico.Messaging.Abstractions;
using Aprico.Xml.Extensions;
using Azure.Messaging.ServiceBus;
using Be.Stateless.Extensions;

namespace Aprico.Messaging.ServiceBus.Xml;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API.")]
public class XmlMessageSerializer : IMessageSerializer<ServiceBusMessage>
{
	#region IMessageSerializer<ServiceBusMessage> Members

	public ServiceBusMessage Serialize<TBody>(
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
		message.ApplicationProperties.Add(ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY, body.GetXmlFullyQualifiedName());
		message.ApplicationProperties.Add(ApplicationPropertyNames.TIMESTAMP_PROPERTY, (timestamp ?? DateTimeOffset.UtcNow).ToString("o"));
		businessId.IfNotNullOrEmpty(bid => message.ApplicationProperties.Add(ApplicationPropertyNames.BUSINESS_ID_PROPERTY, bid));
		return message;
	}

	#endregion
}
