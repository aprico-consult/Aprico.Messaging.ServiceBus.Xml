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
using AutoFixture.Xunit2;
using Azure.Messaging.ServiceBus;

namespace Aprico.Messaging.ServiceBus.Extensions;

public abstract class ServiceBusReceivedMessageExtensionsFixture
{
	#region Nested Type: CopyContextPropertiesTo

	public class CopyContextPropertiesTo : ServiceBusReceivedMessageExtensionsFixture
	{
		[Theory]
		[InlineAutoData(ApplicationPropertyNames.BusinessId, false, true)]
		[InlineAutoData(ApplicationPropertyNames.BusinessId, true, false)]
		[InlineAutoData(ApplicationPropertyNames.MessageBodyType, false, true)]
		[InlineAutoData(ApplicationPropertyNames.MessageBodyType, true, false)]
		[InlineAutoData(ApplicationPropertyNames.Timestamp, false, true)]
		[InlineAutoData(ApplicationPropertyNames.Timestamp, true, false)]
		[InlineAutoData("SomeProperty", false, true)]
		[InlineAutoData("SomeProperty", true, true)]
		public void CanExcludeBusinessContextProperties(
			string contextProperty,
			bool excludeBusinessContextProperties,
			bool isExpectedToBeContainedInCopy,
			string contextPropertyValue,
			ServiceBusMessage target)
		{
			var message = CreateServiceBusReceivedMessage(contextProperty, contextPropertyValue);

			message.CopyContextPropertiesTo(target, excludeBusinessContextProperties, includeDeadLetterContextProperties: false);

			target.ApplicationProperties.ContainsKey(contextProperty)
				.Should()
				.Be(isExpectedToBeContainedInCopy);
		}

		[Theory]
		[InlineAutoData(ApplicationPropertyNames.DeadLetterReason, false, false)]
		[InlineAutoData(ApplicationPropertyNames.DeadLetterReason, true, true)]
		[InlineAutoData(ApplicationPropertyNames.DeadLetterErrorDescription, false, false)]
		[InlineAutoData(ApplicationPropertyNames.DeadLetterErrorDescription, true, true)]
		[InlineAutoData("SomeProperty", false, true)]
		[InlineAutoData("SomeProperty", true, true)]
		public void CanIncludeDeadLetterContextProperties(
			string contextProperty,
			bool includeDeadLetterContextProperties,
			bool isExpectedToBeContainedInCopy,
			string contextPropertyValue,
			ServiceBusMessage target)
		{
			var message = CreateServiceBusReceivedMessage(contextProperty, contextPropertyValue);

			message.CopyContextPropertiesTo(target, excludeBusinessContextProperties: false, includeDeadLetterContextProperties);

			target.ApplicationProperties.ContainsKey(contextProperty)
				.Should()
				.Be(isExpectedToBeContainedInCopy);
		}
	}

	#endregion

	#region Nested Type: GetBusinessId

	public class GetBusinessId : ServiceBusReceivedMessageExtensionsFixture
	{
		[Theory]
		[AutoData]
		public void ReturnsBusinessId(string businessIdValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.BusinessId, businessIdValue);
			message.GetBusinessId()
				.Should()
				.Be(businessIdValue);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).GetBusinessId())
				.Should()
				.Throw<ArgumentNullException>();
		}

		[Fact]
		public void ThrowsWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			Invoking(() => message.GetBusinessId())
				.Should()
				.Throw<InvalidOperationException>();
		}
	}

	#endregion

	#region Nested Type: GetMessageBodyType

	public class GetMessageBodyType : ServiceBusReceivedMessageExtensionsFixture
	{
		[Theory]
		[AutoData]
		public void ReturnsMessageType(string messageType)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.MessageBodyType, messageType);
			message.GetMessageBodyType()
				.Should()
				.Be(messageType);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).GetMessageBodyType())
				.Should()
				.Throw<ArgumentNullException>();
		}

		[Fact]
		public void ThrowsWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			Invoking(() => message.GetMessageBodyType())
				.Should()
				.Throw<InvalidOperationException>();
		}
	}

	#endregion

	#region Nested Type: GetTimestamp

	public class GetTimestamp : ServiceBusReceivedMessageExtensionsFixture
	{
		[Theory]
		[AutoData]
		public void ReturnsTimestamp(DateTimeOffset timestampValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.Timestamp, timestampValue.ToString("o"));
			message.GetTimestamp()
				.Should()
				.Be(timestampValue);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).GetTimestamp())
				.Should()
				.Throw<ArgumentNullException>();
		}

		[Fact]
		public void ThrowsWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			Invoking(() => message.GetTimestamp())
				.Should()
				.Throw<InvalidOperationException>();
		}

		[Theory]
		[AutoData]
		public void ThrowsWhenValueHasWrongFormat(DateTimeOffset timestampValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.Timestamp, timestampValue.ToString("d"));
			Invoking(() => message.GetTimestamp())
				.Should()
				.Throw<FormatException>();
		}
	}

	#endregion

	#region Nested Type: TryGetBusinessId

	public class TryGetBusinessId : ServiceBusReceivedMessageExtensionsFixture
	{
		[Fact]
		public void ReturnsFalseWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			message.TryGetBusinessId(out var businessId)
				.Should()
				.BeFalse();
			businessId.Should()
				.BeNull();
		}

		[Theory]
		[AutoData]
		public void ReturnsTrueAndBusinessId(string businessIdValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.BusinessId, businessIdValue);
			message.TryGetBusinessId(out var businessId)
				.Should()
				.BeTrue();
			businessId.Should()
				.Be(businessIdValue);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).TryGetBusinessId(out _))
				.Should()
				.Throw<ArgumentNullException>();
		}
	}

	#endregion

	#region Nested Type: TryGetMessageBodyType

	public class TryGetMessageBodyType : ServiceBusReceivedMessageExtensionsFixture
	{
		[Fact]
		public void ReturnsFalseWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			message.TryGetMessageBodyType(out _)
				.Should()
				.BeFalse();
		}

		[Theory]
		[AutoData]
		public void ReturnsTrueAndMessageType(string messageType)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.MessageBodyType, messageType);
			message.TryGetMessageBodyType(out var messageTypeValue)
				.Should()
				.BeTrue();
			messageTypeValue.Should()
				.Be(messageType);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).TryGetMessageBodyType(out _))
				.Should()
				.Throw<ArgumentNullException>();
		}
	}

	#endregion

	#region Nested Type: TryGetTimestamp

	public class TryGetTimestamp : ServiceBusReceivedMessageExtensionsFixture
	{
		[Fact]
		public void ReturnsFalseWhenPropertyIsMissing()
		{
			var message = CreateServiceBusReceivedMessage();
			message.TryGetTimestamp(out _)
				.Should()
				.BeFalse();
		}

		[Theory]
		[AutoData]
		public void ReturnsTrueAndTimestamp(DateTimeOffset timestampValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.Timestamp, timestampValue.ToString("o"));
			message.TryGetTimestamp(out var timestamp)
				.Should()
				.BeTrue();
			timestamp.Should()
				.Be(timestampValue);
		}

		[Fact]
		[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
		public void ThrowsWhenMessageIsNull()
		{
			Invoking(static () => ((ServiceBusReceivedMessage) null!).TryGetTimestamp(out _))
				.Should()
				.Throw<ArgumentNullException>();
		}

		[Theory]
		[AutoData]
		public void ThrowsWhenValueHasWrongFormat(DateTimeOffset timestampValue)
		{
			var message = CreateServiceBusReceivedMessage(ApplicationPropertyNames.Timestamp, timestampValue.ToString("d"));
			Invoking(() => message.TryGetTimestamp(out _))
				.Should()
				.Throw<FormatException>();
		}
	}

	#endregion

	private static ServiceBusReceivedMessage CreateServiceBusReceivedMessage()
	{
		return ServiceBusModelFactory.ServiceBusReceivedMessage();
	}

	private static ServiceBusReceivedMessage CreateServiceBusReceivedMessage(string contextProperty, string contextPropertyValue)
	{
		return ServiceBusModelFactory.ServiceBusReceivedMessage(
			properties: new Dictionary<string, object> {
				{ contextProperty, contextPropertyValue }
			});
	}
}
