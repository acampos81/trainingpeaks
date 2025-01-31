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
			// Sort dates in chronological order before using.
			_dates.Sort();

			if(_dates.Count == 0)
			{
				return _data.GetUserWorkoutsByDate(_userID, DateTime.UnixEpoch, DateTime.UtcNow);
			}
			else if(_dates.Count == 1)
			{
				// Search within the 24 hour window of a single date.
				var startDate = _dates[0];
				var endDate   = new DateTimeOffset(startDate).AddHours(24).Date;
				return _data.GetUserWorkoutsByDate(_userID, startDate, endDate);
			}
			else if(_dates.Count == 2)
			{
				// Ensures all 24 hours of end date are covered.
				var endDate   = new DateTimeOffset(_dates[1]).AddHours(24).Date;
				return _data.GetUserWorkoutsByDate(_userID, _dates[0], endDate);
			}

			return new List<Workout>();
		}
	}
}
