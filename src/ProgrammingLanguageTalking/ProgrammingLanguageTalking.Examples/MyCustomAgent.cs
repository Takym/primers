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
		private volatile ReplyNameDecision? _other_name;

		public override string? DisplayName { get; }

		public MyCustomAgent(string? displayName)
		{
			this.DisplayName = displayName;
		}

		protected override Decision? OnMessageReceived(Agent sender, Context context, Decision decision)
		{
			if ((sender, context, decision).IsTalkOrigin()) {
				goto HelloWorldDecision;
			}

			if (decision is HelloWorldDecision) {
				if (context[nameof(HelloWorldDecision)] is null) {
					context[nameof(HelloWorldDecision)] = decision;
					goto HelloWorldDecision;
				}

				goto AskNameDecision;
			}

			if (decision is AskNameDecision) {
				goto ReplyNameDecision;
			}

			if (decision is ReplyNameDecision) {
				if (context[nameof(ReplyNameDecision)] is null) {
					context[nameof(ReplyNameDecision)] = decision;
					goto AskNameDecision;
				}

				goto GoodByeDecision;
			}

			if (decision is GoodByeDecision &&
				context[nameof(GoodByeDecision)] is null) {
				context[nameof(GoodByeDecision)] = decision;
				goto GoodByeDecision;
			}

			return null;

		HelloWorldDecision:
			return new HelloWorldDecision(this, this.MakeDecision);

		AskNameDecision:
			return decision.SendMessage(this, context, new AskNameDecision(this, this.MakeDecision)) switch {
				ReplyNameDecision otherName => _other_name = otherName,
				_                           => null
			};

		ReplyNameDecision:
			return new ReplyNameDecision(this.DisplayName, this, decision.SendMessage);

		GoodByeDecision:
			return new GoodByeDecision(this, NullAgent.Instance.MakeDecision);
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

		public sealed class AskNameDecision(MyCustomAgent receiver, SendMessageFunc sendMessage)
			: Decision("名前質問", receiver, sendMessage);

		public sealed class ReplyNameDecision(string? agentName, MyCustomAgent receiver, SendMessageFunc sendMessage)
			: Decision(string.IsNullOrEmpty(agentName) ? "名前回答" : $"名前回答「{agentName}」", receiver, sendMessage)
		{
			public string? AgentName => agentName;
		}
	}
}
