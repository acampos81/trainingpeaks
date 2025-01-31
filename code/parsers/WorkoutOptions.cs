using CommandLine;

namespace trainingpeaks
{
	[Verb("workout", HelpText = "Look up workout data for one or more users.")]
	public class WorkoutOptions
	{
		[Option('r',
			FlagCounter = true,
			Group       = "Stats Flags",
			HelpText    = "The amount reps completed for an exercise.")]
		public int Reps { get; set; }

		[Option('w',
			FlagCounter = true,
			Group       = "Stats Flags",
			HelpText    = "The amount of weight lifted for an exercise.")]
		public int Weight { get; set; }

		[Option("pr",
			FlagCounter = true,
			SetName     = "Personal Record",
			HelpText    = "The personal record for an exercise.")]
		public int PersonalRecord { get; set; }

		[Option("users",
			Required     = true,
			Separator    = ',',
			HelpText     = "Specify one or more users by ID.\n" +
			"Multiple IDs must be comma delimited (e.g. 1234,9876,4545)"
		)]
		public IEnumerable<int> UserIDs { get; set; }

		[Option("exercise", Required = true, HelpText = "Specify an exercises by ID")]
		public int ExcerciseID { get; set; }

		[Option("dates",
			Min       = 1,
			Max       = 2,
			Separator = ' ',
			HelpText  =  
			"Specify a date, or date range with format: yyyy-mm-dd.\n\t"+
			"Valid ranges:\n\t\t"+
			"2025-01-01            Finds workouts on a specific date.\n\t\t"+
			"2025-01-01 2025-01-31 Finds workouts within a date range."
		)]
		public IEnumerable<string>? Dates { get; set; }

		[Option('W',
			FlagCounter = true,
			HelpText = "Print Warnings")]
		public int PrintWarnings { get; set; }

		[Option('o', HelpText = "Output file path.")]
		public string? OutputPath { get; set; }
	}
}
