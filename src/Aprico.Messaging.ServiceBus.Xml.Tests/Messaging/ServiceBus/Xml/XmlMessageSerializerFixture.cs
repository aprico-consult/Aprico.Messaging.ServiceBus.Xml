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
using Aprico.Messaging.ServiceBus.Xml.Dummies;
using AutoFixture.Xunit2;

namespace Aprico.Messaging.ServiceBus.Xml;

public class XmlMessageSerializerFixture
{
	[Theory]
	[AutoData]
	public void CannotSerializeUnqualifiedContract(XmlMessageSerializer sut)
	{
		Invoking(() => sut.Serialize(new UnqualifiedDummy()))
			.Should()
			.Throw<InvalidOperationException>();
	}

	[Theory]
	[AutoData]
	public void CanSerializeFullyQualifiedContract(XmlMessageSerializer sut)
	{
		var message = sut.Serialize(new FullyQualifiedDummy());
		message.ApplicationProperties[ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY]
			.Should()
			.Be(typeof(FullyQualifiedDummy).GetXmlFullyQualifiedName());
	}

	[Theory]
	[AutoData]
	public void CanSerializePartiallyQualifiedContract(XmlMessageSerializer sut)
	{
		var message = sut.Serialize(new PartiallyQualifiedDummy());
		message.ApplicationProperties[ApplicationPropertyNames.MESSAGE_TYPE_PROPERTY]
			.Should()
			.Be(typeof(PartiallyQualifiedDummy).GetXmlFullyQualifiedName());
	}

	[Theory]
	[AutoData]
	public void SetsMessageId(XmlMessageSerializer sut)
	{
		var message = sut.Serialize(new FullyQualifiedDummy());
		message.MessageId.Should()
			.NotBeNullOrWhiteSpace()
			.And.Match(static s => Guid.Parse(s) != Guid.Empty);
	}

	[Theory]
	[AutoData]
	public void SetsOptionalMetadata(
		string messageId,
		string correlationId,
		string sessionId,
		string businessId,
		DateTimeOffset timestamp,
		DateTimeOffset scheduledEnqueueTime,
		XmlMessageSerializer sut)
	{
		var message = sut.Serialize(new FullyQualifiedDummy(), messageId, correlationId, sessionId, businessId, timestamp, scheduledEnqueueTime);
		message.MessageId.Should()
			.Be(messageId);
		message.CorrelationId.Should()
			.Be(correlationId);
		message.ReplyToSessionId.Should()
			.Be(sessionId);
		message.ApplicationProperties[ApplicationPropertyNames.BUSINESS_ID_PROPERTY]
			.Should()
			.Be(businessId);
		message.ApplicationProperties[ApplicationPropertyNames.TIMESTAMP_PROPERTY]
			.Should()
			.Be(timestamp.ToString("o"));
		message.ScheduledEnqueueTime.Should()
			.Be(scheduledEnqueueTime);
	}

	[Theory]
	[AutoData]
	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
	public void ThrowsWhenBodyIsNull(XmlMessageSerializer sut)
	{
		Invoking(() => sut.Serialize<FullyQualifiedDummy>(null!))
			.Should()
			.Throw<ArgumentNullException>();
	}
}
