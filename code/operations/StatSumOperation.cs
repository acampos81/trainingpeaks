using System.Text;

namespace trainingpeaks
{
    public class StatSumOperation : IDataOperation<float>, ILogWarnings
	{
		public  StringBuilder? Warnings { get; set; }

		private int            _exerciseID;
		private StatFlags      _statFlags;
		private List<Workout>  _workouts;

		public StatSumOperation(int exerciseID, StatFlags statFlags, List<Workout> workouts)
		{
			_exerciseID = exerciseID;
			_statFlags  = statFlags;
			_workouts   = workouts;
		}

		public float Run()
		{
			float statSum = 0f;

			foreach(var wo in _workouts)
			{
				foreach(var bl in wo.blocks)
				{
					if(bl.exercise_id == _exerciseID)
					{
						foreach(var set in bl.sets)
						{
							if(set.reps.HasValue)
							{
								if(set.weight.HasValue)
								{
									if(_statFlags == (StatFlags.Reps | StatFlags.Weight))
									{
										statSum += set.reps.Value * set.weight.Value;
									}
									else if((_statFlags & StatFlags.Reps) > 0)
									{
										statSum += set.reps.Value;
									}
									else
									{
										statSum += set.weight.Value;
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

			return statSum;
		}
	}
}
