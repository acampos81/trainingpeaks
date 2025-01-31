namespace trainingpeaks
{
	[Flags]
	public enum StatFlags
	{
		None   = 0,
		Reps   = 1,
		Weight = 1 << 1,
	}

	public static class StatFlagsExentions
	{
		public static string ToJsonValue(this StatFlags flags)
		{
			if(flags == (StatFlags.Reps | StatFlags.Weight))
			{
				return "total_weight";
			}
			return flags.ToString().ToLower();
		}
	}
}
