/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace ProgrammingLanguageTalking
{
	public class Decision : Word
	{
		public override string?         DisplayName { get; }
		public          Agent           Receiver    { get; }
		public          SendMessageFunc SendMessage { get; }

		public Decision(string? displayName, Agent receiver, SendMessageFunc sendMessage)
		{
			ArgumentNullException.ThrowIfNull(receiver   );
			ArgumentNullException.ThrowIfNull(sendMessage);

			this.DisplayName = displayName;
			this.Receiver    = receiver;
			this.SendMessage = sendMessage;
		}

		public Decision(Agent receiver)
			: this(receiver.DisplayName, receiver, receiver.MakeDecision) { }

		public override string ToString()
			=> $"{base.ToString()}〈受信者：{this.Receiver}〉";

		protected private string BaseToString()
			=> base.ToString();
	}

	public sealed class NullDecision : Decision
	{
		private static readonly NullDecision _inst    =  new();
		public  static          NullDecision Instance => _inst;

		private NullDecision() : base(NullAgent.Instance) { }

		public override string ToString() => this.BaseToString();
	}

	public delegate Decision? SendMessageFunc(Agent sender, Context context, Decision decision);
}
