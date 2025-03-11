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

/// <summary>
/// Provides extension methods for working with Azure <see cref="ServiceBusReceivedMessage"/> and their
/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
/// </summary>
/// <remarks>
/// This static class offers utility methods to manipulate and extract context
/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/> from <see cref="ServiceBusReceivedMessage"/>s, including a.o.
/// <see cref="ApplicationPropertyNames.BusinessId"/>, <see cref="ApplicationPropertyNames.MessageBodyType"/>,
/// <see cref="ApplicationPropertyNames.Timestamp"/>.
/// </remarks>
[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
public static class ServiceBusReceivedMessageExtensions
{
	#region Copy Context Properties

	/// <summary>Copies context properties from a <see cref="ServiceBusReceivedMessage"/> to a <see cref="ServiceBusMessage"/>.</summary>
	/// <param name="source">
	/// The source <see cref="ServiceBusReceivedMessage"/> containing
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/> to copy.
	/// </param>
	/// <param name="target">The target <see cref="ServiceBusMessage"/> where properties will be copied to.</param>
	/// <param name="ignoreBusinessContextProperties">
	/// If <see langword="true"/>, business-related context properties (such as
	/// <see cref="ApplicationPropertyNames.BusinessId"/>, <see cref="ApplicationPropertyNames.MessageBodyType"/>, and
	/// <see cref="ApplicationPropertyNames.Timestamp"/>) will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <param name="ignoreDeadLetterContextProperties">
	/// If <see langword="true"/>, dead-letter related context properties (such as
	/// <see cref="ApplicationPropertyNames.DeadLetterReason"/> and <see cref="ApplicationPropertyNames.DeadLetterErrorDescription"/>)
	/// will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="target"/> is
	/// <see langword="null"/>.
	/// </exception>
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

	/// <summary>
	/// Copies context properties from a <see cref="ServiceBusReceivedMessage"/> to an
	/// <see cref="IDictionary{String, Object}"/>target.
	/// </summary>
	/// <param name="source">
	/// The source <see cref="ServiceBusReceivedMessage"/> containing
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/> to copy.
	/// </param>
	/// <param name="target">The target <see cref="IDictionary{String, Object}"/> where properties will be copied to.</param>
	/// <param name="ignoreBusinessContextProperties">
	/// If <see langword="true"/>, business-related context properties (such as
	/// <see cref="ApplicationPropertyNames.BusinessId"/>, <see cref="ApplicationPropertyNames.MessageBodyType"/>, and
	/// <see cref="ApplicationPropertyNames.Timestamp"/>) will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <param name="ignoreDeadLetterContextProperties">
	/// If <see langword="true"/>, dead-letter related context properties (such as
	/// <see cref="ApplicationPropertyNames.DeadLetterReason"/> and <see cref="ApplicationPropertyNames.DeadLetterErrorDescription"/>)
	/// will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="target"/> is
	/// <see langword="null"/>.
	/// </exception>
	public static void CopyContextPropertiesTo(
		this ServiceBusReceivedMessage source,
		IDictionary<string, object> target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		ArgumentNullException.ThrowIfNull(source);
		source.ApplicationProperties.CopyContextPropertiesTo(target, ignoreBusinessContextProperties, ignoreDeadLetterContextProperties);
	}

	/// <summary>
	/// Copies context properties from an <see cref="IReadOnlyDictionary{String,Object}"/> to a
	/// <see cref="ServiceBusMessage"/>.
	/// </summary>
	/// <param name="source">
	/// The source <see cref="IReadOnlyDictionary{String,Object}"/> containing
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/> to copy.
	/// </param>
	/// <param name="target">The target <see cref="ServiceBusMessage"/> where properties will be copied to.</param>
	/// <param name="ignoreBusinessContextProperties">
	/// If <see langword="true"/>, business-related context properties (such as
	/// <see cref="ApplicationPropertyNames.BusinessId"/>, <see cref="ApplicationPropertyNames.MessageBodyType"/>, and
	/// <see cref="ApplicationPropertyNames.Timestamp"/>) will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <param name="ignoreDeadLetterContextProperties">
	/// If <see langword="true"/>, dead-letter related context properties (such as
	/// <see cref="ApplicationPropertyNames.DeadLetterReason"/> and <see cref="ApplicationPropertyNames.DeadLetterErrorDescription"/>)
	/// will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="target"/> is
	/// <see langword="null"/>.
	/// </exception>
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

	/// <summary>
	/// Copies context properties from an <see cref="IReadOnlyDictionary{String,Object}"/> to an
	/// <see cref="IDictionary{String, Object}"/>.
	/// </summary>
	/// <param name="source">
	/// The source <see cref="IReadOnlyDictionary{String,Object}"/> containing
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/> to copy.
	/// </param>
	/// <param name="target">The target <see cref="IDictionary{String, Object}"/> where properties will be copied to.</param>
	/// <param name="ignoreBusinessContextProperties">
	/// If <see langword="true"/>, business-related context properties (such as
	/// <see cref="ApplicationPropertyNames.BusinessId"/>, <see cref="ApplicationPropertyNames.MessageBodyType"/>, and
	/// <see cref="ApplicationPropertyNames.Timestamp"/>) will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <param name="ignoreDeadLetterContextProperties">
	/// If <see langword="true"/>, dead-letter related context properties (such as
	/// <see cref="ApplicationPropertyNames.DeadLetterReason"/> and <see cref="ApplicationPropertyNames.DeadLetterErrorDescription"/>)
	/// will be excluded from copying. It defaults to <see langword="true"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">
	/// Thrown if either <paramref name="source"/> or <paramref name="target"/> is
	/// <see langword="null"/>.
	/// </exception>
	public static void CopyContextPropertiesTo(
		this IReadOnlyDictionary<string, object> source,
		IDictionary<string, object> target,
		bool ignoreBusinessContextProperties = true,
		bool ignoreDeadLetterContextProperties = true)
	{
		source.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.BusinessId)
			.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.MessageBodyType)
			.Where(kvp => !ignoreBusinessContextProperties || kvp.Key != ApplicationPropertyNames.Timestamp)
			.Where(kvp => !ignoreDeadLetterContextProperties || kvp.Key != ApplicationPropertyNames.DeadLetterReason)
			.Where(kvp => !ignoreDeadLetterContextProperties || kvp.Key != ApplicationPropertyNames.DeadLetterErrorDescription)
			.ForEach(kvp => target.Add(kvp.Key, kvp.Value));
	}

	#endregion

	#region BusinessId

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.BusinessId"/> from a <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">The <see cref="ServiceBusMessage"/> to extract the property from.</param>
	/// <returns>
	/// The value of the <see cref="ApplicationPropertyNames.BusinessId"/> stored in the <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the property is not defined in the <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </exception>
	public static string GetBusinessId(this ServiceBusMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.GetBusinessId();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.BusinessId"/> from a <see cref="ServiceBusReceivedMessage"/>'s
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">The <see cref="ServiceBusReceivedMessage"/> to extract the property from.</param>
	/// <returns>
	/// The value of the <see cref="ApplicationPropertyNames.BusinessId"/> stored in the
	/// <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the property is not defined in the
	/// <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </exception>
	public static string GetBusinessId(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetBusinessId();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.BusinessId"/> from an
	/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties.
	/// </summary>
	/// <param name="properties">The properties dictionary to extract the <see cref="ApplicationPropertyNames.BusinessId"/> from.</param>
	/// <returns>
	/// The value of the <see cref="ApplicationPropertyNames.BusinessId"/> stored in the
	/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the <see cref="ApplicationPropertyNames.BusinessId"/> property is not
	/// defined in the <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties.
	/// </exception>
	public static string GetBusinessId(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetBusinessId(out var businessId)) return businessId;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.BusinessId} property.");
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.BusinessId"/> from a <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusMessage"/> to extract the <see cref="ApplicationPropertyNames.BusinessId"/>
	/// from.
	/// </param>
	/// <param name="businessId">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.BusinessId"/> if found,
	/// otherwise <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.BusinessId"/> is successfully retrieved; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetBusinessId(this ServiceBusMessage message, [NotNullWhen(returnValue: true)] out string? businessId)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.TryGetBusinessId(out businessId);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.BusinessId"/> from a
	/// <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> to extract the
	/// <see cref="ApplicationPropertyNames.BusinessId"/> from.
	/// </param>
	/// <param name="businessId">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.BusinessId"/> if found,
	/// otherwise <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.BusinessId"/> is successfully retrieved; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetBusinessId(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out string? businessId)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetBusinessId(out businessId);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.BusinessId"/> from an
	/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties.
	/// </summary>
	/// <param name="properties">The properties dictionary to extract the <see cref="ApplicationPropertyNames.BusinessId"/> from.</param>
	/// <param name="businessId">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.BusinessId"/> if found,
	/// otherwise <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.BusinessId"/> is successfully retrieved; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	public static bool TryGetBusinessId(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out string? businessId)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetValue(ApplicationPropertyNames.BusinessId, out var value))
		{
			businessId = (string) value;
			return true;
		}
		businessId = null;
		return false;
	}

	#endregion

	#region MessageBodyType

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.MessageBodyType"/> from a <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusMessage"/> to extract the <see cref="ApplicationPropertyNames.MessageBodyType"/>
	/// from.
	/// </param>
	/// <returns>
	/// The <see cref="ApplicationPropertyNames.MessageBodyType"/> stored in the
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static string GetMessageBodyType(this ServiceBusMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.GetMessageBodyType();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.MessageBodyType"/> from a <see cref="ServiceBusReceivedMessage"/>'s
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> to extract the
	/// <see cref="ApplicationPropertyNames.MessageBodyType"/> from.
	/// </param>
	/// <returns>
	/// The <see cref="ApplicationPropertyNames.MessageBodyType"/> stored in the
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static string GetMessageBodyType(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetMessageBodyType();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.MessageBodyType"/> from an
	/// <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties.
	/// </summary>
	/// <param name="properties">
	/// The <see cref="IReadOnlyDictionary{TKey, TValue}"/> of properties to extract the
	/// <see cref="ApplicationPropertyNames.MessageBodyType"/> from.
	/// </param>
	/// <returns>The <see cref="ApplicationPropertyNames.MessageBodyType"/> stored in the dictionary properties.</returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the <see cref="ApplicationPropertyNames.MessageBodyType"/> property is
	/// not defined.
	/// </exception>
	public static string GetMessageBodyType(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetMessageBodyType(out var messageType)) return messageType;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.MessageBodyType} property.");
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.MessageBodyType"/> from a <see cref="ServiceBusMessage"/>
	/// 's <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusMessage"/> to extract the <see cref="ApplicationPropertyNames.MessageBodyType"/>
	/// from.
	/// </param>
	/// <param name="messageBodyType">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.MessageBodyType"/> if
	/// found in the <see cref="ServiceBusMessage"/>'s <see cref="ServiceBusMessage.ApplicationProperties"/>, otherwise
	/// <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.MessageBodyType"/> is successfully retrieved;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetMessageBodyType(this ServiceBusMessage message, [NotNullWhen(returnValue: true)] out string? messageBodyType)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.TryGetMessageBodyType(out messageBodyType);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.MessageBodyType"/> from a
	/// <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> to extract the
	/// <see cref="ApplicationPropertyNames.MessageBodyType"/> from.
	/// </param>
	/// <param name="messageBodyType">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.MessageBodyType"/> if
	/// found in the <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>, otherwise
	/// <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.MessageBodyType"/> is successfully retrieved;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetMessageBodyType(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out string? messageBodyType)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetMessageBodyType(out messageBodyType);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.MessageBodyType"/> from an
	/// <see cref="IReadOnlyDictionary{String, Object}"/> of properties.
	/// </summary>
	/// <param name="properties">The dictionary of properties to search for the <see cref="ApplicationPropertyNames.MessageBodyType"/>.</param>
	/// <param name="messageBodyType">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.MessageBodyType"/> if
	/// found; otherwise, <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.MessageBodyType"/> is successfully retrieved;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	public static bool TryGetMessageBodyType(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out string? messageBodyType)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetValue(ApplicationPropertyNames.MessageBodyType, out var value))
		{
			messageBodyType = (string) value;
			return true;
		}
		messageBodyType = null;
		return false;
	}

	#endregion

	#region Timestamp

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.Timestamp"/> from a <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">The <see cref="ServiceBusMessage"/> to extract the <see cref="ApplicationPropertyNames.Timestamp"/> from.</param>
	/// <returns>
	/// The <see cref="ApplicationPropertyNames.Timestamp"/> stored in the
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static DateTimeOffset GetTimestamp(this ServiceBusMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.GetTimestamp();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.Timestamp"/> from a <see cref="ServiceBusReceivedMessage"/>'s
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> to extract the
	/// <see cref="ApplicationPropertyNames.Timestamp"/> from.
	/// </param>
	/// <returns>
	/// The <see cref="ApplicationPropertyNames.Timestamp"/> stored in the
	/// <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static DateTimeOffset GetTimestamp(this ServiceBusReceivedMessage message)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.GetTimestamp();
	}

	/// <summary>
	/// Retrieves the <see cref="ApplicationPropertyNames.Timestamp"/> from an
	/// <see cref="IReadOnlyDictionary{String, Object}"/> of properties.
	/// </summary>
	/// <param name="properties">
	/// The <see cref="IReadOnlyDictionary{String, Object}"/> of properties to extract the
	/// <see cref="ApplicationPropertyNames.Timestamp"/> from.
	/// </param>
	/// <returns>The <see cref="ApplicationPropertyNames.Timestamp"/> value as a <see cref="DateTimeOffset"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no <see cref="ApplicationPropertyNames.Timestamp"/> property is found in
	/// the <see cref="IReadOnlyDictionary{String, Object}"/> of properties.
	/// </exception>
	public static DateTimeOffset GetTimestamp(this IReadOnlyDictionary<string, object> properties)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (properties.TryGetTimestamp(out var timestamp)) return (DateTimeOffset) timestamp;
		throw new InvalidOperationException(
			$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)} does not define a {ApplicationPropertyNames.Timestamp} property.");
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.Timestamp"/> from a <see cref="ServiceBusMessage"/>'s
	/// <see cref="ServiceBusMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">The <see cref="ServiceBusMessage"/> to extract the <see cref="ApplicationPropertyNames.Timestamp"/> from.</param>
	/// <param name="timestamp">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.Timestamp"/> if found;
	/// otherwise, <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.Timestamp"/> is successfully retrieved; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetTimestamp(this ServiceBusMessage message, [NotNullWhen(returnValue: true)] out DateTimeOffset? timestamp)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.AsReadOnly()
			.TryGetTimestamp(out timestamp);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.Timestamp"/> from a
	/// <see cref="ServiceBusReceivedMessage"/>'s <see cref="ServiceBusReceivedMessage.ApplicationProperties"/>.
	/// </summary>
	/// <param name="message">
	/// The <see cref="ServiceBusReceivedMessage"/> to extract the
	/// <see cref="ApplicationPropertyNames.Timestamp"/> from.
	/// </param>
	/// <param name="timestamp">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.Timestamp"/> if found;
	/// otherwise, <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.Timestamp"/> is successfully retrieved; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is <see langword="null"/>.</exception>
	public static bool TryGetTimestamp(this ServiceBusReceivedMessage message, [NotNullWhen(returnValue: true)] out DateTimeOffset? timestamp)
	{
		ArgumentNullException.ThrowIfNull(message);
		return message.ApplicationProperties.TryGetTimestamp(out timestamp);
	}

	/// <summary>
	/// Attempts to retrieve the <see cref="ApplicationPropertyNames.Timestamp"/> from an
	/// <see cref="IReadOnlyDictionary{String, Object}"/> of properties.
	/// </summary>
	/// <param name="properties">
	/// The <see cref="IReadOnlyDictionary{String, Object}"/> of properties to extract the
	/// <see cref="ApplicationPropertyNames.Timestamp"/> from.
	/// </param>
	/// <param name="timestamp">
	/// When this method returns, contains the <see cref="ApplicationPropertyNames.Timestamp"/> if found and
	/// successfully parsed; otherwise, <see langword="null"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="ApplicationPropertyNames.Timestamp"/> is successfully retrieved and parsed;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if the <paramref name="properties"/> dictionary is <see langword="null"/>.</exception>
	/// <exception cref="FormatException">
	/// Thrown if the <see cref="ApplicationPropertyNames.Timestamp"/> value cannot be parsed as a
	/// valid <see cref="DateTimeOffset"/>.
	/// </exception>
	public static bool TryGetTimestamp(this IReadOnlyDictionary<string, object> properties, [NotNullWhen(returnValue: true)] out DateTimeOffset? timestamp)
	{
		ArgumentNullException.ThrowIfNull(properties);
		if (!properties.TryGetValue(ApplicationPropertyNames.Timestamp, out var value))
		{
			timestamp = null;
			return false;
		}
		if (!DateTimeOffset.TryParseExact(value as string, "o", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var result))
			throw new FormatException(
				$"{nameof(ServiceBusReceivedMessage)}.{nameof(ServiceBusReceivedMessage.ApplicationProperties)}.{ApplicationPropertyNames.Timestamp} format exception." + Environment.NewLine
				+ $"The string '{value}' was not recognized as a valid {nameof(DateTimeOffset)}.");
		timestamp = result;
		return true;
	}

	#endregion
}
