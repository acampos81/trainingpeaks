using System.Text.Json.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using trainingpeaks;

namespace tests
{
	[TestClass]
	public class ProcessTests
	{
		[TestMethod]
		public void PersonalRecordProcess()
		{
			var statFlags = StatFlags.Weight;

			var userIDs = new List<int>()
			{
				MockData.TestUserID_A,
				MockData.TestUserID_B
			};

			var workoutsA = MockData.GetWorkoutsUserA();
			var workoutsB = MockData.GetWorkoutsUserB();

			var testID    = MockData.TestExerciseID_A;
			var testPr    = 10000f;

			// Set the same personal record both user workouts on the same block
			workoutsA[0].blocks[0] = new ExerciseBlock
			{ 
				exercise_id = testID,
				sets        = new List<Set>()
				{
					new Set { reps = 1, weight = testPr }
				}
			};
			workoutsB[0].blocks[0] = new ExerciseBlock
			{ 
				exercise_id = testID,
				sets        = new List<Set>()
				{
					new Set { reps = 1, weight = testPr }
				}
			};

			var userWorkouts = new Dictionary<int, List<Workout>>()
			{
				{ userIDs[0], workoutsA },
				{ userIDs[1], workoutsB }
			};

			var dataSrc = MockData.GetDataSource();
			var jsonStr = ProcessFunctions.ProcessPersonalRecords(userIDs, testID, statFlags, userWorkouts, dataSrc, null);
			var jsonObj = JsonObject.Parse(jsonStr);

			var outID    = (int)jsonObj!["exercise"]!["id"]!.AsValue();
			var outUsers = jsonObj!["users"]!.AsArray();
			var prA      = (float)outUsers[0]!["personal_record"]!.AsValue();
			var prB      = (float)outUsers[1]!["personal_record"]!.AsValue();

			Assert.AreEqual(outID, testID);
			Assert.AreEqual(outUsers.Count,  userIDs.Count, "JSON user count does not match input users");
			Assert.AreEqual(prA, testPr, $"Output personal record for user {userIDs[0]} did not match.");
			Assert.AreEqual(prB, testPr, $"Output personal record for user {userIDs[1]} did not match.");
		}

		[TestMethod]
		public void TotalWeightProcess()
		{
			// Flags indicate reps * weight
			var statFlags = StatFlags.Reps | StatFlags.Weight;

			var userIDs = new List<int>()
			{
				MockData.TestUserID_A,
				MockData.TestUserID_B
			};

			// Get the mock data for both users
			var workoutsA = MockData.GetWorkoutsUserA();
			var workoutsB = MockData.GetWorkoutsUserB();

			// Limit test to a single workout per user
			var testWorkoutA = workoutsA[0];
			var testWorkoutB = workoutsB[0];

			// Create new blocks for both workouts
			testWorkoutA.blocks = new List<ExerciseBlock>();
			testWorkoutB.blocks = new List<ExerciseBlock>();

			var testID    = MockData.TestExerciseID_B;
			var testTw    = 1000;

			// Create a block with a known total weight.
			var testBlock = new ExerciseBlock
			{ 
				exercise_id = testID,
				sets        = new List<Set>()
				{
					new Set { reps = 2, weight = 250 }
				}
			};

			// Set the same block for both user workouts
			workoutsA[0].blocks.Add(testBlock);
			workoutsB[0].blocks.Add(testBlock);

			// Include only test workouts only.
			var testWorkouts = new List<Workout>()
			{
				testWorkoutA,
				testWorkoutB
			};

			// Create a dictionary with the test workouts
			var userWorkouts = new Dictionary<int, List<Workout>>()
			{
				{ userIDs[0], new List<Workout>(){ testWorkoutA } },
				{ userIDs[1], new List<Workout>(){ testWorkoutB } },
			};

			var dataSrc = new DataSource(MockData.GetUsers(), MockData.GetExercises(), testWorkouts);
			var jsonStr = ProcessFunctions.ProcessStatTotal(userIDs, testID, statFlags, userWorkouts, dataSrc, null);
			var jsonObj = JsonObject.Parse(jsonStr);

			var outID    = (int)jsonObj!["exercise"]!["id"]!.AsValue();
			var outUsers = jsonObj!["users"]!.AsArray();
			var twA      = (float)outUsers[0]![statFlags.ToJsonValue()]!.AsValue();
			var twB      = (float)outUsers[1]![statFlags.ToJsonValue()]!.AsValue();
			var twAB     = twA + twB;

			Assert.AreEqual(outID, testID);
			Assert.AreEqual(outUsers.Count,  userIDs.Count, "JSON user count does not match input users");
			Assert.AreEqual(twA,  500, $"Output total weight for user {userIDs[0]} did not match.");
			Assert.AreEqual(twB,  500, $"Output total weight for user {userIDs[1]} did not match.");
			Assert.AreEqual(twAB, testTw, $"Output combined total weight did not match.");
		}
	}
}
