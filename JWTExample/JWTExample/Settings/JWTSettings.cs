namespace JWTExample.Settings
{
	public class JWTSettings
	{
		public const string SectionName = "JWT";
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public string Key { get; set; }
		public int LifeTime { get; set; }
	}
}
