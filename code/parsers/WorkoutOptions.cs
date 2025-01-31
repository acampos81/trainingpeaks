using CommandLine;

namespace trainingpeaks
{
	[Verb("workout", HelpText = "Look up workout data for one or more users.")]
	public class WorkoutOptions
	{
		[Option("tw",
			FlagCounter = true,
			Required    = true,
			SetName     = "Total Weight",
			HelpText    = "The total weight (reps * weight) for an exercise.")]
		public int TotalWeight { get; set; }

		[Option("pr",
			FlagCounter = true,
			Required    = true,
			SetName     = "Personal Record",
			HelpText    = "The most weight lifted for an exercise, regarldess of reps.")]
		public int PersonalRecord { get; set; }

		[Option('u', "users",
			Required     = true,
			Separator    = ',',
			HelpText     = "Specify one or more users by ID.\n" +
			"Multiple IDs must be comma delimeted (e.g. 1234,9876,4545)"
		)]
		public IEnumerable<int> UserIDs { get; set; }

		[Option('e', "exercise", Required = true, HelpText = "Specify an exercises by ID")]
		public int ExcerciseID { get; set; }

		[Option('d', "dates",
			Min       = 1,
			Max       = 2,
			Separator = ' ',
			HelpText  =  
			"Specify a date, or date range with format: yyyy-mm-dd.\n\t"+
			"Valid ranges:\n\t\t"+
			"2025-01-01            Finds workouts on a specific date.\n\t\t"+
			"2025-01-01 2025-01-31 Finds workouts within a date range.\n\t\t"
		)]
		public IEnumerable<string>? Dates { get; set; }

		[Option('w',
			FlagCounter = true,
			HelpText = "Print Warnings")]
		public int PrintWarnings { get; set; }

		[Option('o', "output", HelpText = "Output file path.")]
		public string? OutputPath { get; set; }
	}
}
