/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProgrammingLanguageTalking
{
	public class Context : Word
	{
		public override string?                                 DisplayName { get; }
		public          RootContext                             Root        { get; }
		public          Context?                                Parent      { get; }
		public          ConcurrentDictionary<object, Decision?> Decisions   { get; }

		public Decision? this[object key]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this.GetDecision(key);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => this.Decisions.AddOrUpdate(
				key,
				(_,    value) => value,
				(_, _, value) => value,
				value
			);
		}

		public Context(string? displayName, Context parent)
		{
			if (this is RootContext root) {
				Debug.Assert(parent is null);

				this.Root = root;
			} else {
				ArgumentNullException.ThrowIfNull(parent);

				this.Root = parent.Root;
			}

			this.DisplayName = displayName;
			this.Parent      = parent;
			this.Decisions   = new();
		}

		public virtual Context CreateChild(string? displayName)
			=> new(displayName, this);

		public virtual Decision? GetDecision(object key)
		{
			ArgumentNullException.ThrowIfNull(key);

			if (this.Decisions.TryGetValue(key, out var result)) {
				return result;
			}

			return this.Parent?.GetDecision(key);
		}
	}

	public sealed class RootContext : Context
	{
		public RootContext(string? displayName) : base(displayName, null!) { }
	}
}
