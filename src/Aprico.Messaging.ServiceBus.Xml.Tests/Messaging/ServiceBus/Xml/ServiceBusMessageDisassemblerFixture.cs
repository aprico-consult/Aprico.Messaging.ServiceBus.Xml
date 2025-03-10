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
using Aprico.Messaging.Message.Deserializer;
using Aprico.Messaging.ServiceBus.Xml.Dummies;
using Aprico.Xml.Extensions;
using Azure.Messaging.ServiceBus;

namespace Aprico.Messaging.ServiceBus.Xml;

public class ServiceBusMessageDisassemblerFixture
{
	[Fact]
	public void DeserializeRegisteredXmlContract()
	{
		ServiceBusMessageDisassembler sut = new(MessageContractRegistry.RegisterContract<FullyQualifiedDummy>());
		sut.DeserializeBody(BuildServiceBusReceivedMessage<FullyQualifiedDummy>())
			.Should()
			.BeOfType<FullyQualifiedDummy>();
	}

	[Fact]
	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
	public void ThrowsWhenMessageIsNull()
	{
		ServiceBusMessageDisassembler sut = new(MessageContractRegistry);
		Invoking(() => sut.DeserializeBody(null!))
			.Should()
			.Throw<ArgumentNullException>();
	}

	[Fact]
	public void ThrowsWhenXmlContractIsNotRegistered()
	{
		ServiceBusMessageDisassembler sut = new(MessageContractRegistry);
		Invoking(() => sut.DeserializeBody(BuildServiceBusReceivedMessage<PartiallyQualifiedDummy>()))
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

	private MessageContractRegistry MessageContractRegistry { get; } = new(static type => type.GetXmlFullyQualifiedName());
}
