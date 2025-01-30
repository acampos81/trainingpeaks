namespace trainingpeaks
{
    public class FindSetsOperation : IDataOperation<List<Set>>
	{
		private int     _exerciseID;
		private Workout _workout;

		public FindSetsOperation(int exerciseID, Workout workout)
		{
			_exerciseID = exerciseID;
			_workout    = workout;
		}

		public List<Set> Run()
		{
			var sets = new List<Set>();
			foreach(var bl in _workout.blocks)
			{
				if(bl.exercise_id == _exerciseID)
				{
					sets.AddRange(bl.sets);
				}
			}
			return sets;
		}
	}
}
