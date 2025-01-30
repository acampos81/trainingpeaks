using System.Text.Json;

namespace trainingpeaks
{
	class Program
	{
		static void Main(string[] args)
		{
			var users     = JsonSerializer.Deserialize<List<User>>(File.ReadAllText("../data/users.json"));
			//var exercises = JsonSerializer.Deserialize<Dictionary<string, object>[]>(File.ReadAllText("../data/exercises.json"));
			//var workouts  = JsonSerializer.Deserialize<Dictionary<string, object>[]>(File.ReadAllText("../data/workouts.json"));

			for(int i=0; i<users.Count; i++)
			{
				Console.WriteLine(users[0].name_first);
			}
		}
	}
}