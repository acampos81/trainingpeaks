using Microsoft.VisualStudio.TestTools.UnitTesting;
using trainingpeaks;

namespace tests
{
	[TestClass]
	public class OperationTests
	{
		[TestMethod]
		public void FindAllWorkouts()
		{
			var dataSrc = MockData.GetDataSource();
			var userID  = MockData.TestUserID_C;
			var dates   = new List<DateTime>(){ DateTime.UnixEpoch, DateTime.Now };

			var findWorkoutsOp = new FindUserWorkoutsOperation(0, dates, dataSrc);
			var  workouts      = findWorkoutsOp.Run();
			Assert.AreEqual(workouts.Count, 0, "Invalid user should return empty workout list");

			findWorkoutsOp = new FindUserWorkoutsOperation(userID, dates, dataSrc);
			workouts       = findWorkoutsOp.Run();
			Assert.AreEqual(workouts.Count, 0, $"User ID {userID} has no workout data but returned non-empty list.");

			userID         = MockData.TestUserID_A;
			findWorkoutsOp = new FindUserWorkoutsOperation(userID, dates, dataSrc);
			workouts       = findWorkoutsOp.Run();
			Assert.IsTrue(workouts.Count > 0, "Users with workout data should return lists with count greater than 0.");
			Assert.AreEqual(workouts.Count, 3, $"User ID {userID} should have workout list with count equal to 3.");
		}

		[TestMethod]
		public void FindWorkoutByDate()
		{
			var dataSrc   = MockData.GetDataSource();
			var userID    = MockData.TestUserID_C;
			var startDate = DateTime.Parse("2025-01-01");
			var endDate   = DateTime.Parse("2025-01-15");
			var dates     = new List<DateTime>(){ startDate, endDate };

			var findWorkoutsOp = new FindUserWorkoutsOperation(userID, dates, dataSrc);
			var workouts       = findWorkoutsOp.Run();
			Assert.AreEqual(workouts.Count, 0, $"User ID {userID} has no workouts on dates specified.");

			// Single day search.
			userID         = MockData.TestUserID_A;
			dates[0]       = startDate;
			dates[1]       = startDate;
			findWorkoutsOp = new FindUserWorkoutsOperation(userID, dates, dataSrc);
			workouts       = findWorkoutsOp.Run();
			Assert.AreEqual(workouts.Count, 1, $"User ID {userID} should have 1 workout on {dates[0]}");

			// Use date range out of chronoligical order, operation should sort them.
			userID         = MockData.TestUserID_A;
			dates[0]       = endDate;
			dates[1]       = startDate;
			findWorkoutsOp = new FindUserWorkoutsOperation(userID, dates, dataSrc);
			workouts       = findWorkoutsOp.Run();

			Assert.IsTrue(dates[0].Equals(startDate) && dates[1].Equals(endDate), "Operation dates are out of order.");
			Assert.AreEqual(workouts.Count, 2, $"User ID {userID} should have 2 workout between dates {dates[0]} {dates[1]}");
		}

		[TestMethod]
		public void FindPersonalRecord()
		{
			var workouts = MockData.GetWorkoutsUserA();
			var testPr   = 10000f;
			var testID   = workouts[0].blocks[0].exercise_id;

			// Set a known personal record for this test
			workouts[0].blocks[0].sets = new List<Set>()
			{
				// Valid set
				new Set
				{
					reps   = 10,
					weight = testPr
				},
				// Reps are 0, weight should not be counted
				new Set
				{
					reps   = 0,
					weight = testPr + 1f,
				},
				// Invalid reps, weight should not be counted.
				new Set
				{
					reps   = null,
					weight = testPr * 2f
				},
				// Invalid weight should not be counted.
				new Set
				{
					reps   = 1,
					weight = null
				}
			};

			var prOp      = new PersonalRecordOperation(testID, workouts);
			prOp.Warnings = new System.Text.StringBuilder();
			var pr        = prOp.Run();
			int warning1  = prOp.Warnings.ToString().IndexOf("Warning");
			int warning2  = prOp.Warnings.ToString().LastIndexOf("Warning");

			Assert.AreEqual(pr, testPr, $"Personal record {pr} does not match expected value of {testPr}");
			Assert.IsFalse(string.IsNullOrEmpty(prOp.Warnings.ToString()), "No warnings were generated for invalid data");
			Assert.IsTrue(warning1 != warning2, "Two expected warnings were not generated for invalid data");
		}

		[TestMethod]
		public void FindTotalWeight()
		{
			var workouts = MockData.GetWorkoutsUserA();

			// Use a single test workout and set a known total weight
			var testList = new List<Workout>(){workouts[0]};
			var testID   = 1;
			var testTw   = 300;

			// Set a known number of exercise blocks for this test
			testList[0].blocks = new List<ExerciseBlock>()
			{
				new ExerciseBlock
				{
					exercise_id = testID,

					// Set a known total weight for this test
					sets = new List<Set>()
					{
						new Set
						{
							reps   = 10,
							weight = 10
						},
						new Set
						{
							reps   = 10,
							weight = 20,
						},
						// Invalid reps, weight should not be counted.
						new Set
						{
							reps   = null,
							weight = 100
						},
						// Invalid weight should not be counted.
						new Set
						{
							reps   = 1,
							weight = null
						}
					}
				}
			};

			var twOp      = new TotalWeightOperation(testID, testList);
			twOp.Warnings = new System.Text.StringBuilder();
			var tw        = twOp.Run();
			int warning1  = twOp.Warnings.ToString().IndexOf("Warning");
			int warning2  = twOp.Warnings.ToString().LastIndexOf("Warning");

			Assert.AreEqual(tw, testTw, $"Total weight {tw} does not match expected value of {testTw}");
			Assert.IsFalse(string.IsNullOrEmpty(twOp.Warnings.ToString()), "No warnings were generated for invalid data");
			Assert.IsTrue(warning1 != warning2, "Two expected warnings were not generated for invalid data");
		}
	}
}
