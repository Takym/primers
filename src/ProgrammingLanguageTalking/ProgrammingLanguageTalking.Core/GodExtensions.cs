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
using static ProgrammingLanguageTalking.God;

namespace ProgrammingLanguageTalking
{
	public static class GodExtensions
	{
		private static volatile God _default = Instance;

		public static God Default
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _default;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				ArgumentNullException.ThrowIfNull(value);
				_default = value;
			}
		}

		public static SacredGreatValue<string> GratefulText
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _default.DisplayName.GetString();
		}

		public static void PrayFor<T>(this God god, T? obj)
			=> god.GetString(obj?.ToString() ?? string.Empty).Print();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SacredGreatValue<bool> GetTrue()
			=> _default.GetTrue();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SacredGreatValue<bool> GetFalse()
			=> _default.GetFalse();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SacredGreatValue<int> GetInteger(this int value)
			=> _default.GetInteger(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SacredGreatValue<string> GetString(this string value)
			=> _default.GetString(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LogValue<T>(this StringBuilder sb, SacredGreatValue<T> value)
			=> _default.LogValue(sb, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LogValue<T>(this TextWriter tw, SacredGreatValue<T> value)
			=> _default.LogValue(tw, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Print<T>(this SacredGreatValue<T> value)
			=> _default.LogValue(Console.Out, value);

		public static void Print(this bool value)
			=> (value ? GetTrue() : GetFalse()).Print();

		public static void Print(this int value)
			=> value.GetInteger().Print();

		public static void Print(this string value)
			=> value.GetString().Print();
	}
}
