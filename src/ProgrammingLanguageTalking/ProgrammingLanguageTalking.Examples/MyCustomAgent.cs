/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

namespace ProgrammingLanguageTalking.Examples
{
	public class MyCustomAgent : Agent
	{
		protected override Decision? OnMessageReceived(Agent sender, Context context, Decision decision)
		{
			if (context is RootContext && context.Decisions.IsEmpty) {
				return sender.MakeDecision(this, context, decision);
			}

			return null;
		}
	}
}
