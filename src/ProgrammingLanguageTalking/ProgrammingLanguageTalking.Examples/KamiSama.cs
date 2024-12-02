namespace ProgrammingLanguageTalking.Examples
{
	public sealed class KamiSama : OuterDeity
	{
		private      static readonly KamiSama _inst    =  new();
		internal new static          KamiSama Instance => _inst;

		private KamiSama() { }
	}
}
