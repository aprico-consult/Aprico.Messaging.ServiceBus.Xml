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
using Aprico.Extensions;
using Aprico.Messaging.ServiceBus.Xml.Dummies;
using Aprico.Xml.Extensions;
using AutoFixture.Xunit2;
using Azure.Messaging.ServiceBus;

namespace Aprico.Messaging.ServiceBus.Xml;

public class XmlMessageDeserializerFixture
{
	[Theory]
	[AutoData]
	public void DeserializeRegisteredXmlContract(XmlMessageDeserializer sut)
	{
		var message = BuildServiceBusReceivedMessage<FullyQualifiedDummy>();
		var body = sut.AddXmlContract<FullyQualifiedDummy>()
			.DeserializeBody(message);
		body.Should()
			.BeOfType<FullyQualifiedDummy>();
	}

	[Theory]
	[AutoData]
	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
	public void ThrowsWhenMessageIsNull(XmlMessageDeserializer sut)
	{
		Invoking(() => sut.DeserializeBody(null!))
			.Should()
			.Throw<ArgumentNullException>();
	}

	[Theory]
	[AutoData]
	public void ThrowsWhenXmlContractIsNotRegistered(XmlMessageDeserializer sut)
	{
		var message = BuildServiceBusReceivedMessage<PartiallyQualifiedDummy>();

		Invoking(() => sut.DeserializeBody(message))
			.Should()
			.Throw<InvalidOperationException>();
	}

	private static ServiceBusReceivedMessage BuildServiceBusReceivedMessage<T>()
		where T : notnull, new()
	{
		var message = ServiceBusModelFactory.ServiceBusReceivedMessage(
			new BinaryData(new T().SerializeAsXmlBinary()),
			properties: new Dictionary<string, object> {
				{ ApplicationPropertyNames.MessageBodyType, typeof(T).GetXmlFullyQualifiedName() }
			});
		return message;
	}
}
