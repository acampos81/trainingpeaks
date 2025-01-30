using System.Text;

namespace trainingpeaks
{
    public class PersonalRecordOperation : IDataOperation<float>, ILogWarnings
	{
		public  StringBuilder? Warnings { get; set; }

		private int            _exerciseID;
		private List<Workout>  _workouts;

		public PersonalRecordOperation(int exerciseID, List<Workout> workouts)
		{
			_exerciseID = exerciseID;
			_workouts   = workouts;
		}

		public float Run()
		{
			float highestWeight = 0f;

			foreach(var wo in _workouts)
			{
				foreach(var bl in wo.blocks)
				{
					if(bl.exercise_id == _exerciseID)
					{
						foreach(var set in bl.sets)
						{
							if(set.weight.HasValue)
							{
								if(set.weight.Value > highestWeight)
								{
									highestWeight = set.weight.Value;
								}
							}
							else
							{
								Warnings?.AppendLine($"[Warning] workout from {wo.datetime_completed} for exercise {bl.exercise_id} has invalid weight.");
							}
						}
					}
				}
			}

			return highestWeight;
		}
	}
}
