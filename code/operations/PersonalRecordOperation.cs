using System.Text;

namespace trainingpeaks
{
    public class PersonalRecordOperation : IDataOperation<(float, DateTime)>, ILogWarnings
	{
		public  StringBuilder? Warnings { get; set; }

		private int            _exerciseID;
		private StatFlags      _statFlags;
		private List<Workout>  _workouts;

		public PersonalRecordOperation(int exerciseID, StatFlags statFlags, List<Workout> workouts)
		{
			_exerciseID = exerciseID;
			_statFlags  = statFlags;
			_workouts   = workouts;
		}

		public (float, DateTime) Run()
		{
			(float, DateTime) record = (0f, default(DateTime));

			foreach(var wo in _workouts)
			{
				foreach(var bl in wo.blocks)
				{
					if(bl.exercise_id == _exerciseID)
					{
						foreach(var set in bl.sets)
						{
							// Ensure the set has a valid reps value
							if(set.reps.HasValue)
							{
								// Ensure the set has a valid weight value.
								if(set.weight.HasValue)
								{
									// The reps of an exercise need to be greater than zero to be considered for a personal best.
									if(set.reps.Value > 0)
									{
										float setValue = 0f;
										if(_statFlags == (StatFlags.Reps | StatFlags.Weight))
										{
											setValue = set.reps.Value * set.weight.Value;
										}
										else if((_statFlags & StatFlags.Reps) > 0)
										{
											setValue = set.reps.Value;
										}
										else
										{
											setValue = set.weight.Value;
										}

										// Set the workout date and record value if the current highest value has been surpassed.
										if(setValue > record.Item1)
										{
											record.Item1 = setValue;
											record.Item2 = wo.datetime_completed;
										}
									}
									else
									{
										Warnings?.AppendLine($"[Warning] workout from {wo.datetime_completed} for exercise {bl.exercise_id} has 0 reps.");
									}
								}
								else
								{
									Warnings?.AppendLine($"[Warning] workout from {wo.datetime_completed} for exercise {bl.exercise_id} has invalid weight.");
								}
							}
							else
							{
								Warnings?.AppendLine($"[Warning] workout from {wo.datetime_completed} for exercise {bl.exercise_id} has invalid reps.");
							}
						}
					}
				}
			}

			return record;
		}
	}
}
