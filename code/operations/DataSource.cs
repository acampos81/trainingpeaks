using System.Text.Json;

namespace trainingpeaks
{
    public class DataSource
	{
		// Main dictionaries keyed by ID
		private Dictionary<int, User>           _usersByID;
		private Dictionary<int, Exercise>       _exercisesByID;
		private Dictionary<int, List<Workout>>  _workoutsByUserID;

		// Name to ID convenience dictionaries
		private Dictionary<string, int>   _usersIDsByName;
		private Dictionary<string, int>   _exerciseIDsByName;

		public DataSource()
		{
			// JSON serialization options to parse data format.
			var jsonOps = new JsonSerializerOptions();
			jsonOps.IncludeFields = true;
			jsonOps.Converters.Add(new DateConverter());

			var users     = JsonSerializer.Deserialize<List<User>>(File.ReadAllText("data/users.json"),         jsonOps);
			var exercises = JsonSerializer.Deserialize<List<Exercise>>(File.ReadAllText("data/exercises.json"), jsonOps);
			var workouts  = JsonSerializer.Deserialize<List<Workout>>(File.ReadAllText("data/workouts.json"),   jsonOps);

			BuildUserLookups(users!);
			BuildExerciseLookups(exercises!);
			BuildWorkoutLookups(workouts!);
		}


		private void BuildUserLookups(List<User> users)
		{
			_usersByID      = new Dictionary<int, User>();
			_usersIDsByName = new Dictionary<string, int>();

			for (int i = 0; i < users.Count; i++)
			{
				var user = users[i];
				_usersByID.Add(user.id, user);
				_usersIDsByName.Add($"{user.name_first} {user.name_last}", user.id);
			}
		}

		private void BuildExerciseLookups(List<Exercise> exercises)
		{
			_exercisesByID     = new Dictionary<int, Exercise>();
			_exerciseIDsByName = new Dictionary<string, int>();

			for (int i = 0; i < exercises.Count; i++)
			{
				var ex = exercises[i];
				_exercisesByID.Add(ex.id, ex);
				_exerciseIDsByName.Add(ex.title!, ex.id);
			}
		}

		private void BuildWorkoutLookups(List<Workout> workouts)
		{
			_workoutsByUserID = new Dictionary<int, List<Workout>>();

			for (int i = 0; i < workouts.Count; i++)
			{
				var wo     = workouts[i];
				int userID = wo.user_id; 
				if(_workoutsByUserID.ContainsKey(userID))
				{
					_workoutsByUserID[userID].Add(wo);
				}
				else
				{
					_workoutsByUserID.Add(wo.user_id, new List<Workout>(){ wo });
				}
			}
		}

		public bool TryGetUserByID(int userID, out User user, out string msg)
		{
			user = default(User);
			msg  = string.Empty;

			if(_usersByID.TryGetValue(userID, out user))
			{
				return true;
			}

			msg = $"Unable to find user with ID {userID}";
			return false;
		}

		public bool TryGetExerciseByID(int exerciseID, out Exercise excercise, out string msg)
		{
			excercise = default(Exercise);
			msg  = string.Empty;

			if(_exercisesByID.TryGetValue(exerciseID, out excercise))
			{
				return true;
			}

			msg = $"Unable to find exercise with ID {exerciseID}";
			return false;
		}

		public bool TryGetUserIDByName(string userName, out int userID, out string firstLast, out string msg)
		{
			return TryGetIDByName(_usersIDsByName, userName, out userID, out firstLast, out msg);
		}

		public bool TryGetExerciseIDByName(string exerciseName, out int exerciseID, out string fullName, out string msg)
		{
			return TryGetIDByName(_exerciseIDsByName, exerciseName, out exerciseID, out fullName, out msg);
		}

		private bool TryGetIDByName(Dictionary<string, int> dictionary, string searchName, out int id, out string foundName, out string msg)
		{
			id                   = -1;
			foundName            = string.Empty;
			msg                  = string.Empty;
			string lowerName     = searchName.ToLower();
			List<string> matches = new List<string>();

			foreach(var kvp in dictionary)
			{
				if(kvp.Key.ToLower().Contains(lowerName))
				{
					matches.Add(kvp.Key);
				}
			}

			if(matches.Count == 1)
			{
				foundName = matches[0];
				id = dictionary[foundName];
				return true;
			}
			else if(matches.Count > 1)
			{
				msg = $"Multiple matches found with name {searchName}";
			}
			else if(matches.Count == 0)
			{
				msg = $"Unable to find any matches with name {searchName}";
			}

			return false;
		}

		public List<Workout> GetUserWorkoutsByDate(int userID, DateTime startDate, DateTime endDate)
		{
			List<Workout> userWorkouts = new List<Workout>();

			if(_workoutsByUserID.TryGetValue(userID, out List<Workout> workouts))
			{
				foreach(Workout wo in workouts)
				{
					if( wo.datetime_completed.CompareTo(startDate) >= 0 && 
						wo.datetime_completed.CompareTo(endDate)   <= 0)
					{
						userWorkouts.Add(wo);
					}
				}
			}

			return userWorkouts;
		}
	}
}
