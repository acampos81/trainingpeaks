using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace trainingpeaks
{
	public static class ProcessFunctions
	{
		public static string ProcessPersonalRecords(
			List<int>                      userIDs,
			int                            exerciseID,
			StatFlags                      statFlags,
			Dictionary<int, List<Workout>> userWorkouts,
			DataSource                     dataSrc,
			StringBuilder                  warnings)
		{
			dataSrc.TryGetExerciseByID(exerciseID, out Exercise exercise, out _);

			var jsonOptions = new JsonSerializerOptions{ WriteIndented = true };
			var jsonOut     = JsonObject.Parse("{}");
				
			jsonOut!["exercise"] = JsonObject.Parse(JsonSerializer.Serialize(exercise, jsonOptions));
			jsonOut!["users"]    = new JsonArray();

			foreach(var uID in userIDs)
			{
				var workouts = userWorkouts[uID];
				var prOp      = new PersonalRecordOperation(exerciseID, statFlags, workouts);
				prOp.Warnings = warnings;
				var pr        = prOp.Run();

				dataSrc.TryGetUserByID(uID, out User user, out _);

				var userJson = JsonObject.Parse(JsonSerializer.Serialize(user, jsonOptions));
				userJson!["personal_record"] = pr.Item1;
				userJson!["record_stat"]     = statFlags.ToJsonValue();
				userJson!["record_date"]     = pr.Item2.ToString("MMMM dd yyyy");
					
				jsonOut!["users"]!.AsArray().Add(userJson);
			}

			return jsonOut.ToJsonString(new JsonSerializerOptions{WriteIndented=true});
		}

		public static string ProcessStatTotal(
			List<int>                      userIDs,
			int                            exerciseID,
			StatFlags                      statFlags,
			Dictionary<int, List<Workout>> userWorkouts,
			DataSource                     dataSrc,
			StringBuilder                  warnings)
		{
			dataSrc.TryGetExerciseByID(exerciseID, out Exercise exercise, out _);

			var jsonOptions = new JsonSerializerOptions{ WriteIndented = true };
			var jsonOut     = JsonObject.Parse("{}");
				
			jsonOut!["exercise"] = JsonObject.Parse(JsonSerializer.Serialize(exercise, jsonOptions));
			jsonOut!["users"]    = new JsonArray();

			var sum = 0f;
			foreach(var uID in userIDs)
			{
				var workouts    = userWorkouts[uID];
				var sumOp       = new StatSumOperation(exerciseID, statFlags, workouts);
				sumOp.Warnings  = warnings;
				var userSum     = sumOp.Run();
				sum            += userSum;

				dataSrc.TryGetUserByID(uID, out User user, out _);

				var userJson = JsonObject.Parse(JsonSerializer.Serialize(user, jsonOptions));
				userJson![statFlags.ToJsonValue()] = userSum;
					
				jsonOut!["users"]!.AsArray().Add(userJson);
			}

			jsonOut["group_total"] = sum;

			return jsonOut.ToJsonString(new JsonSerializerOptions{WriteIndented=true});
		}
	}
}
