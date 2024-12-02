/****
 * プログラミング言語会話
 * Copyright (C) 2024 Takym.
 *
 * distributed under the MIT License.
****/

namespace ProgrammingLanguageTalking
{
	public abstract class Word
	{
		public virtual string? DisplayName => null;

		public override string ToString()
		{
			var t = this.GetType();

			string? dn = this.DisplayName;
			string  tn = t.FullName ?? t.Name;

			if (string.IsNullOrEmpty(tn)) {
				tn = "任意の型";
			}

			return string.IsNullOrEmpty(dn) ? tn : $"{dn}（{tn}）";
		}
	}
}
