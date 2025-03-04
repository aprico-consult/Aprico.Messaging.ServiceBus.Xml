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

using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;

namespace Aprico.Messaging.ServiceBus;

/// <summary>Provides constant string definitions for context <see cref="ServiceBusMessage.ApplicationProperties"/>.</summary>
/// <remarks>
/// This static class centralizes the naming of common <see cref="ServiceBusMessage.ApplicationProperties"/> across
/// different message-related contexts, such as business context and dead letter handling.
/// </remarks>
[SuppressMessage("ReSharper", "MemberCanBeInternal")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("Style", "IDE1006:Naming Styles")]
public static class ApplicationPropertyNames
{
	#region Business Context Properties

	/// <summary>Represents a unique business identifier for a message.</summary>
	public const string BusinessId = nameof(BusinessId);

	/// <summary>Represents the type of the message body.</summary>
	public const string MessageBodyType = nameof(MessageBodyType);

	/// <summary>Represents the timestamp associated with the message.</summary>
	public const string Timestamp = nameof(Timestamp);

	#endregion

	#region Dead Letter Context Properties

	/// <summary>Represents a detailed description of the error that caused a message to be dead-lettered.</summary>
	public const string DeadLetterErrorDescription = nameof(DeadLetterErrorDescription);

	/// <summary>Represents the primary reason for a message being dead-lettered.</summary>
	public const string DeadLetterReason = nameof(DeadLetterReason);

	#endregion
}
