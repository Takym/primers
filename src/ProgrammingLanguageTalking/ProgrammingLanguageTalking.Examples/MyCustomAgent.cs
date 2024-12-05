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
		public override string? DisplayName { get; }

		public MyCustomAgent(string? displayName)
		{
			this.DisplayName = displayName;
		}

		protected override Decision? OnMessageReceived(Agent sender, Context context, Decision decision)
		{
			if (decision is GoodByeDecision) {
				return null;
			}

			if ((sender, context, decision).IsTalkOrigin()) {
				goto HelloWorldDecision;
			}

			if (decision is HelloWorldDecision) {
				if (context[nameof(HelloWorldDecision)] is null) {
					context[nameof(HelloWorldDecision)] = decision;
					goto HelloWorldDecision;
				}

				return new GoodByeDecision(this, NullAgent.Instance.MakeDecision);
			}

			return null;

		HelloWorldDecision:
			return new HelloWorldDecision(this, this.MakeDecision);
		}

		public abstract class GreetDecision : Decision
		{
			public string? Text { get; }

			public GreetDecision(string? text, MyCustomAgent receiver, SendMessageFunc sendMessage)
				: base(string.IsNullOrEmpty(text) ? "挨拶" : $"挨拶「{text}」", receiver, sendMessage)
			{
				this.Text = text;
			}
		}

		public sealed class HelloWorldDecision : GreetDecision
		{
			public HelloWorldDecision(MyCustomAgent receiver, SendMessageFunc sendMessage)
				: base("Hello, World!!", receiver, sendMessage) { }
		}

		public sealed class GoodByeDecision : GreetDecision
		{
			public GoodByeDecision(MyCustomAgent receiver, SendMessageFunc sendMessage)
				: base("good-bye...", receiver, sendMessage) { }
		}
	}
}
