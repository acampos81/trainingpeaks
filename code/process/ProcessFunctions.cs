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
				var prOp      = new PersonalRecordOperation(exerciseID, workouts);
				prOp.Warnings = warnings;
				var pr        = prOp.Run();

				dataSrc.TryGetUserByID(uID, out User user, out _);

				var userJson = JsonObject.Parse(JsonSerializer.Serialize(user, jsonOptions));
				userJson!["personal_record"] = pr;
					
				jsonOut!["users"]!.AsArray().Add(userJson);
			}

			return jsonOut.ToJsonString(new JsonSerializerOptions{WriteIndented=true});
		}

		public static string ProcessTotalWeight(
			List<int>                      userIDs,
			int                            exerciseID,
			Dictionary<int, List<Workout>> userWorkouts,
			DataSource                     dataSrc,
			StringBuilder                  warnings)
		{
			dataSrc.TryGetExerciseByID(exerciseID, out Exercise exercise, out _);

			var jsonOptions = new JsonSerializerOptions{ WriteIndented = true };
			var jsonOut     = JsonObject.Parse("{}");
				
			jsonOut!["exercise"] = JsonObject.Parse(JsonSerializer.Serialize(exercise, jsonOptions));
			jsonOut!["users"]    = new JsonArray();

			var tw = 0f;
			foreach(var uID in userIDs)
			{
				var workouts   = userWorkouts[uID];
				var twOp       = new TotalWeightOperation(exerciseID, workouts);
				twOp.Warnings  = warnings;
				var userTw     = twOp.Run();
				tw            += userTw;

				dataSrc.TryGetUserByID(uID, out User user, out _);

				var userJson = JsonObject.Parse(JsonSerializer.Serialize(user, jsonOptions));
				userJson!["total_weight"] = userTw;
					
				jsonOut!["users"]!.AsArray().Add(userJson);
			}

			jsonOut["combined_total_weight"] = tw;

			return jsonOut.ToJsonString(new JsonSerializerOptions{WriteIndented=true});
		}
	}
}
