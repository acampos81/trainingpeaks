using System.Text;
using CommandLine;
using CommandLine.Text;

namespace trainingpeaks
{
    class Program
	{
		static void Main(string[] args)
		{
			var dataSrc  = new DataSource();

			// parser setting to override default help text generator.
			var parser = new Parser((config) => { config.HelpWriter = null; config.GetoptMode = true; });
			
			// parse arguments
			var argResults = parser.ParseArguments<IDOptions, WorkoutOptions>(args);

			// run successfully parsed options
			argResults.WithParsed<IDOptions>(ops => RunIDOptions(dataSrc, ops));
			argResults.WithParsed<WorkoutOptions>(ops => RunWorkoutOptions(dataSrc, ops));

			// show help if requested, or on errors.
			argResults.WithNotParsed((argErrors) => { PrintHelp(argResults, argErrors); });
		}

		static void RunIDOptions(DataSource dataSrc, IDOptions idOps)
		{
			var uNames = idOps.UserNames!.ToList();
			if(uNames.Count > 0)
			{
				string name = string.Join(' ',  uNames);
				if (dataSrc.TryGetUserIDByName(name, out int userID, out string firstLast, out string msg))
				{
					Console.WriteLine($"{firstLast} ID: {userID}");
				}
				else
				{
					throw new Exception(msg);
				}
			}

			var eNames = idOps.ExerciseNames!.ToList();
			if(eNames.Count > 0)
			{
				string name = string.Join(' ', eNames);
				if (dataSrc.TryGetExerciseIDByName(name, out int exerciseID, out string fullName, out string msg))
				{
					Console.WriteLine($"{fullName} ID: {exerciseID}");
				}
				else
				{
					throw new Exception(msg);
				}
			}
		}

		static void RunWorkoutOptions(DataSource dataSrc, WorkoutOptions workoutOps)
		{
			StringBuilder warnings = new StringBuilder();

			var userIDs = new List<int>();
			foreach(var id in workoutOps.UserIDs!)
			{
				if(userIDs.Contains(id) == false)
				{
					userIDs.Add(id);
				}
				else
				{
					warnings.AppendLine($"[Warning] Input duplicate ID \"{id}\" was ignored.");
				}
			}

			var exerciseID = workoutOps.ExcerciseID;
			
			var dateStrings = workoutOps.Dates.ToList();
			var dates       = new List<DateTime>();
			if(dateStrings.Count > 0)
			{
				foreach(var dStr in dateStrings)
				{
					dates.Add(DateTime.Parse(dStr));
				}
				dates.Sort();
			}

			var userWorkouts = new Dictionary<int, List<Workout>>();
			foreach(var uID in userIDs)
			{
				if (userWorkouts.ContainsKey(uID) == false)
				{
					var findWorkoutsOp  = new FindUserWorkoutsOperation(uID, dates, dataSrc);
					var workouts        = findWorkoutsOp.Run();
					userWorkouts.Add(uID, workouts);
				}
			}

			var jsonStr  = string.Empty;
			var fileName = string.Empty;
			if(workoutOps.PersonalRecord != 0)
			{
				fileName = "personal_records.json";
				jsonStr  = ProcessFunctions.ProcessPersonalRecords(userIDs, exerciseID, userWorkouts, dataSrc, warnings);
			}
			else if(workoutOps.TotalWeight != 0)
			{
				fileName = "total_weight.json";
				jsonStr  = ProcessFunctions.ProcessTotalWeight(userIDs, exerciseID, userWorkouts, dataSrc, warnings);
			}

			if(jsonStr.Length > 0)
			{
				if(string.IsNullOrEmpty(workoutOps.OutputPath) == false)
				{
					FileWriter.WriteJson(workoutOps.OutputPath, fileName ,jsonStr);
				}
				Console.WriteLine($"output:\n{jsonStr}");
			}

			if(workoutOps.PrintWarnings != 0 && warnings.Length > 0)
			{
				PrintErrors(warnings);
			}
		}

		static void PrintHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
		{
			HelpText helpText  = HelpText.AutoBuild(result);
			helpText.AutoVersion = false;
			helpText.Heading     = "TraningPeaks";
			helpText.Copyright   = "Angel Campos";
			Console.WriteLine(helpText);
		}

		static void PrintErrors(StringBuilder errors)
		{
			if(errors.Length > 0)
			{
				Console.WriteLine($"\nERRORS:");
				Console.WriteLine(errors.ToString());
			}
		}

		static void PrintWarnings(StringBuilder warnings)
		{
			if(warnings.Length > 0)
			{
				Console.WriteLine($"\nWARNINGS:");
				Console.WriteLine(warnings.ToString());
			}
		}
	}
}