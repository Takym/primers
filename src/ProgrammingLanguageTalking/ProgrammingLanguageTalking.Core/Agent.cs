/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace ProgrammingLanguageTalking
{
	public abstract class Agent : Word
	{
		public Decision? MakeDecision(Agent sender, Context context, Decision decision)
		{
			ArgumentNullException.ThrowIfNull(sender  );
			ArgumentNullException.ThrowIfNull(context );
			ArgumentNullException.ThrowIfNull(decision);

			if (sender == this) {
				throw new ArgumentException($"The specified sender is this agent. ({this})", nameof(sender));
			}

			return this.OnMessageReceived(sender, context, decision);
		}

		protected abstract Decision? OnMessageReceived(Agent sender, Context context, Decision decision);
	}

	public sealed class NullAgent : Agent
	{
		private static readonly NullAgent _inst    =  new();
		public  static          NullAgent Instance => _inst;

		public override string? DisplayName => "***";

		private NullAgent() { }

		protected override Decision OnMessageReceived(Agent sender, Context context, Decision decision)
			=> NullDecision.Instance;
	}
}
