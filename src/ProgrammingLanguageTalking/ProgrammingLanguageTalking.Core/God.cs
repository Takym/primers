/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ProgrammingLanguageTalking
{
	/// <summary>
	///  いわゆる神クラス。
	///  <see href="https://takym.github.io/blog/general/2024/03/13/god-class.html"/>を改良して作った型。
	/// </summary>
	public class God : Word
	{
		private static          ulong  _next_id    = 0;
		private static readonly God    _inst       =  new();
		public  static          God    Instance    => _inst;
		public  sealed override string DisplayName { get; }

		protected private God()
		{
			this.DisplayName = $"参照型大御神【{Interlocked.Increment(ref _next_id):X16}】";
		}

		public virtual SacredGreatValue<bool  > GetTrue   (            ) => new(this, true );
		public virtual SacredGreatValue<bool  > GetFalse  (            ) => new(this, false);
		public virtual SacredGreatValue<int   > GetInteger(int    value) => new(this, value);
		public virtual SacredGreatValue<string> GetString (string value) => new(this, value);

		public virtual void LogValue<T>(StringBuilder sb, SacredGreatValue<T> value)
		{
			ArgumentNullException.ThrowIfNull(sb);

			using (var sw = new StringWriter(sb)) {
				this.LogValue(sw, value);
			}
		}

		public virtual void LogValue<T>(TextWriter tw, SacredGreatValue<T> value)
		{
			ArgumentNullException.ThrowIfNull(tw);

			value.LogCore(this, tw);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override string ToString() => base.ToString();

		public readonly record struct SacredGreatValue<T>
		{
			private readonly God? _owner;
			private readonly T?   _value;

			public God Owner => _owner ?? throw new NullReferenceException();
			public T?  Value => _value;

			internal SacredGreatValue(God owner, T? value)
			{
				ArgumentNullException.ThrowIfNull(owner);

				_owner = owner;
				_value = value;
			}

			internal void LogCore(God god, TextWriter tw)
			{
				var owner = _owner;
				var value = _value;

				if (owner is null) {
					try {
						_ = this.Owner;
					} catch (NullReferenceException nre) {
						throw new InvalidOperationException(nre.Message, nre);
					}
				}

				tw.Write(
					"{0}{1}【{2:yyyy年MM月dd日HH時mm分ss.fffffff秒K}】 ",
					owner!.DisplayName?[6..],
					god   .DisplayName?[6..],
					DateTime.Now
				);

				tw.WriteLine(
					owner == god
						? "神聖偉大値：{0}"
						: "汚物邪悪値：{0}",
					value
				);
			}
		}
	}

	public abstract class OuterDeity : God;
}
