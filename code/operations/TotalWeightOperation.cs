using System.Text;

namespace trainingpeaks
{
    public class TotalWeightOperation : IDataOperation<float>, ILogWarnings
	{
		public  StringBuilder? Warnings { get; set; }

		private int            _exerciseID;
		private List<Workout>  _workouts;

		public TotalWeightOperation(int exerciseID, List<Workout> workouts)
		{
			_exerciseID = exerciseID;
			_workouts   = workouts;
		}

		public float Run()
		{
			float totalWeight = 0f;

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
									totalWeight += set.reps.Value * set.weight.Value;
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

			return totalWeight;
		}
	}
}
