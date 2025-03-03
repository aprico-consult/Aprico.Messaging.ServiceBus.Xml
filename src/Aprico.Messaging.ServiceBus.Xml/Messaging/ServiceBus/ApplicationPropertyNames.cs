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

namespace Aprico.Messaging.ServiceBus;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
[SuppressMessage("ReSharper", "MemberCanBeInternal")]
public static class ApplicationPropertyNames
{
	#region Business Context Properties

	public const string BUSINESS_ID_PROPERTY = "BusinessId";
	public const string MESSAGE_TYPE_PROPERTY = "MessageType";
	public const string TIMESTAMP_PROPERTY = "Timestamp";

	#endregion

	#region Dead Letter Context Properties

	public const string DEAD_LETTER_ERROR_DESCRIPTION_PROPERTY = "DeadLetterErrorDescription";
	public const string DEAD_LETTER_REASON_PROPERTY = "DeadLetterReason";

	#endregion
}
