using CommandLine;

namespace trainingpeaks
{
	[Verb("get-id", HelpText = "Looks up IDs of users or exercises.")]
	public class IDOptions
	{
		[Option('u', "user",
			Min      = 1,
			Max      = 2,
			SetName  = "User ID",
			HelpText =
			"Specify user by full or partial name.\n\t" +
			"Valid formats:\n\t\t" +
			"- Jon Doe\n\t\t" +
			"- John\n\t\t" +
			"- Doe")]
		public IEnumerable<string> UserNames { get; set; }

		[Option('e', "exercise",
			Min      = 1,
			Max      = 2,
			SetName  = "Exercise ID",
			HelpText =
			"Specify exercises by full or partial name.\n\t" +
			"Valid formats:\n\t\t" +
			"- Lat Pulldown\n\t\t" +
			"- Lat\n\t\t- Pulldown")]
		public IEnumerable<string> ExerciseNames { get; set; }
	}
}
