namespace trainingpeaks
{
    public class FindUserWorkoutsOperation : IDataOperation<List<Workout>>
	{
		private int            _userID;
		private List<DateTime> _dates;
		private DataSource     _data;

		public FindUserWorkoutsOperation(int userID, List<DateTime> dates, DataSource dataSrc)
		{
			_userID  = userID;
			_dates   = dates;
			_data    = dataSrc;
		}

		public List<Workout> Run()
		{
			if(_dates.Count == 0)
			{
				return _data.GetUserWorkoutsByDate(_userID, DateTime.UnixEpoch, DateTime.UtcNow);
			}
			else if(_dates.Count == 1)
			{
				return _data.GetUserWorkoutsByDate(_userID, _dates[0], _dates[0]);
			}
			else if(_dates.Count == 2)
			{
				return _data.GetUserWorkoutsByDate(_userID, _dates[0], _dates[1]);
			}

			return new List<Workout>();
		}
	}
}
