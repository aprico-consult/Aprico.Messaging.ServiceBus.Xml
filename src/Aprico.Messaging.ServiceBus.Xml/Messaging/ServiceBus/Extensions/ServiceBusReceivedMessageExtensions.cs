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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Azure.Messaging.ServiceBus;
using Be.Stateless.Linq.Extensions;

namespace Aprico.Messaging.ServiceBus.Extensions;

[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
public static class ServiceBusReceivedMessageExtensions
{
	#region Copy Context Properties

	public static void CopyContextPropertiesTo(
		this ServiceBusReceivedMessage source,
		ServiceBusMessage target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(target);
		source.ApplicationProperties.CopyContextPropertiesTo(target.ApplicationProperties, ignoreBusinessContextProperties, ignoreDeadLetterContextProperties);
	}

	public static void CopyContextPropertiesTo(
		this ServiceBusReceivedMessage source,
		IDictionary<string, object> target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		ArgumentNullException.ThrowIfNull(source);
		source.ApplicationProperties.CopyContextPropertiesTo(target, ignoreBusinessContextProperties, ignoreDeadLetterContextProperties);
	}

	public static void CopyContextPropertiesTo(
		this IReadOnlyDictionary<string, object> source,
		ServiceBusMessage target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(target);
		source.CopyContextPropertiesTo(target.ApplicationProperties, ignoreBusinessContextProperties, ignoreDeadLetterContextProperties);
	}

	public static void CopyContextPropertiesTo(
		this IReadOnlyDictionary<string, object> source,
		IDictionary<string, object> target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		source.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.BUSINESS_ID_PROPERTY)
			.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY)
			.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.TIMESTAMP_PROPERTY)
			.Where(kvp => !ignoreDeadLetterContextProperties || kvp.Key != ApplicationPropertyNames.DEAD_LETTER_REASON_PROPERTY)
			.Where(kvp => !ignoreDeadLetterContextProperties || kvp.Key != ApplicationPropertyNames.DEAD_LETTER_ERROR_DESCRIPTION_PROPERTY)
			.ForEach(kvp => target.Add(kvp.Key, kvp.Value));
	}

	#endregion

	#region BusinessId

	public static string GetBusinessId(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetBusinessId();
	}

	public static string GetBusinessId(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetBusinessId(out var businessId)) return businessId;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.BUSINESS_ID_PROPERTY} property.");
	}

	public static bool TryGetBusinessId(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out string? businessId)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetBusinessId(out businessId);
	}

	public static bool TryGetBusinessId(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out string? businessId)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetValue(ApplicationPropertyNames.BUSINESS_ID_PROPERTY, out var value))
		{
			businessId = (string) value;
			return true;
		}
		businessId = null;
		return false;
	}

	#endregion

	#region MessageType

	public static string GetMessageType(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetMessageType();
	}

	public static string GetMessageType(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetMessageType(out var messageType)) return messageType;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY} property.");
	}

	public static bool TryGetMessageType(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out string? messageType)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetMessageType(out messageType);
	}

	public static bool TryGetMessageType(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out string? messageType)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetValue(ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY, out var value))
		{
			messageType = (string) value;
			return true;
		}
		messageType = null;
		return false;
	}

	#endregion

	#region Timestamp

	public static DateTimeOffset GetTimestamp(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetTimestamp();
	}

	public static DateTimeOffset GetTimestamp(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetTimestamp(out var timestamp)) return (DateTimeOffset) timestamp;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.TIMESTAMP_PROPERTY} property.");
	}

	public static bool TryGetTimestamp(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out DateTimeOffset? timestamp)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetTimestamp(out timestamp);
	}

	public static bool TryGetTimestamp(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out DateTimeOffset? timestamp)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (!properties.TryGetValue(ApplicationPropertyNames.TIMESTAMP_PROPERTY, out var value))
		{
			timestamp = null;
			return false;
		}
		if (!DateTimeOffset.TryParseExact(value as string, "o", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var result))
			throw new FormatException(
				$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)}.{ApplicationPropertyNames.TIMESTAMP_PROPERTY} format exception."
				+ Environment.NewLine + $"The string '{value}' was not recognized as a valid {nameof(DateTimeOffset)}.");
		timestamp = result;
		return true;
	}

	#endregion
}
