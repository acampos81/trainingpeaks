using trainingpeaks;

namespace tests
{
	public static class MockData
	{
		public const int    TestUserID_A        = 1234;
		public const string TestUserFirst_A     = "Jon";
		public const string TestUserLast_A      = "Doe";
		public const int    TestUserID_B        = 5678;
		public const string TestUserFirst_B     = "Jane";
		public const string TestUserLast_B      = "Jones";
		public const int    TestUserID_C        = 9012;
		public const string TestUserFirst_C     = "Sleepy";
		public const string TestUserLast_C      = "McLazy";

		public const int    TestExerciseID_A    = 110;
		public const string TestExerciseTitle_A = "Deltoid Press";
		public const int    TestExerciseID_B    = 111;
		public const string TestExerciseTitle_B = "Leg Press";

		public static DataSource GetDataSource()
		{
			var users     = GetUsers();
			var exercises = GetExercises();
			var workouts  = GetAllWorkouts();
			return new DataSource(users, exercises, workouts);
		}

		public static List<User> GetUsers()
		{
			return new List<User>()
			{
				new User
				{
					id         = TestUserID_A,
					name_first = TestUserFirst_A,
					name_last  = TestUserLast_A
				},
				new User
				{
					id         = TestUserID_B,
					name_first = TestUserFirst_B,
					name_last  = TestUserLast_B
				},
				new User
				{
					id         = TestUserID_C,
					name_first = TestUserFirst_C,
					name_last  = TestUserLast_C
				}
			};
		}

		public static List<Exercise> GetExercises()
		{
			return new List<Exercise>()
			{
				new Exercise
				{
					id    = TestExerciseID_A,
					title = TestExerciseTitle_A
				},
				new Exercise
				{
					id    = TestExerciseID_B,
					title = TestExerciseTitle_B
				}
			};
		}

		public static List<Workout> GetWorkoutsUserA()
		{
			return new List<Workout>
			{
				new Workout
				{
					user_id            = TestUserID_A,
					datetime_completed = DateTime.Parse("2025-01-01"),
					blocks = new List<ExerciseBlock>
					{
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_A,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 10,
									weight = 120,
								},
								new Set
								{
									reps   = 5,
									weight = 150
								},
								new Set
								{
									reps   = null,
									weight = 300
								}
							}
						},
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_B,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 10,
									weight = 20,
								},
								new Set
								{
									reps   = 5,
									weight = 0
								},
								new Set
								{
									reps   = 1000,
									weight = null
								}
							}
						}
					}
				},
				new Workout
				{
					user_id            = TestUserID_A,
					datetime_completed = DateTime.Parse("2025-01-15"),
					blocks = new List<ExerciseBlock>
					{
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_A,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 5,
									weight = 70,
								}
							}
						},
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_B,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 1,
									weight = 50,
								}
							}
						}
					}
				},
				new Workout
				{
					user_id            = TestUserID_A,
					datetime_completed = DateTime.Parse("2025-01-31"),
					blocks = new List<ExerciseBlock>
					{
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_A,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 5,
									weight = 170,
								}
							}
						},
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_B,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 15,
									weight = 80,
								}
							}
						}
					}
				}
			};
		}

		public static List<Workout> GetWorkoutsUserB()
		{
			return new List<Workout>
			{
				new Workout
				{
					user_id            = TestUserID_B,
					datetime_completed = DateTime.Parse("2025-02-01"),
					blocks = new List<ExerciseBlock>
					{
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_A,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 10,
									weight = 100,
								},
								new Set
								{
									reps   = 5,
									weight = 110
								},
								new Set
								{
									reps   = null,
									weight = 120
								}
							}
						},
						new ExerciseBlock
						{
							exercise_id = TestExerciseID_B,
							sets = new List<Set>()
							{
								new Set
								{
									reps   = 10,
									weight = 20,
								},
								new Set
								{
									reps   = 5,
									weight = 30
								},
								new Set
								{
									reps   = 1000,
									weight = null
								}
							}
						}
					}
				},
			};
		}

		public static List<Workout> GetAllWorkouts()
		{
			var allWorkouts = new List<Workout>();
			allWorkouts.AddRange(GetWorkoutsUserA());
			allWorkouts.AddRange(GetWorkoutsUserB());
			return allWorkouts;
		}
	}
}
