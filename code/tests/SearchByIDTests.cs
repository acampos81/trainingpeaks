using Microsoft.VisualStudio.TestTools.UnitTesting;
using trainingpeaks;

namespace tests
{
	[TestClass]
	public class SearchByIDTests
	{
		[TestMethod]
		public void UserIDFound()
		{
			var dataSrc   = MockData.GetDataSource();
			var userID    = MockData.TestUserID_B;
			var userFound = dataSrc.TryGetUserByID(userID, out User user, out string msg);

			Assert.IsTrue(userFound, $"User {userID} was not found.");
			Assert.IsTrue(user.id == userID, $"Output user ID does not match {userID}.");
			Assert.AreEqual(user.name_first, MockData.TestUserFirst_B, "First name does not match.");
			Assert.AreEqual(user.name_last, MockData.TestUserLast_B, "Last name does not match.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == true, "Message was not empty on success.");
		}

		[TestMethod]
		public void UserIDNotFound()
		{
			var dataSrc   = MockData.GetDataSource();
			var userID    = 1010;
			var userFound = dataSrc.TryGetUserByID(userID, out User user, out string msg);

			Assert.IsFalse(userFound, $"Invalid user ID should not be found.");
			Assert.IsTrue(user.id == 0, "User ID is not 0");
			Assert.IsTrue(string.IsNullOrEmpty(user.name_first), "First name is not empty.");
			Assert.IsTrue(string.IsNullOrEmpty(user.name_last), "Last name is not empty.");
			Assert.AreEqual(msg, $"Unable to find user with ID {userID}", "Message doesn't match on failure.");
		}

		[TestMethod]
		public void ExerciseIDFound()
		{
			var dataSrc = MockData.GetDataSource();
			var exID    = MockData.TestExerciseID_A;
			var exFound = dataSrc.TryGetExerciseByID(exID, out Exercise exercise, out string msg);

			Assert.IsTrue(exFound, $"Exercise {exID} was not found.");
			Assert.IsTrue(exercise.id == exID, $"Output excerise ID does not match ID {exID}.");
			Assert.AreEqual(exercise.title, MockData.TestExerciseTitle_A, "Exercise title does not match.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == true, "Message was not empty on success.");
		}

		[TestMethod]
		public void ExcerciseIDNotFound()
		{
			var dataSrc = MockData.GetDataSource();
			var exID    = 0;
			var exFound = dataSrc.TryGetExerciseByID(exID, out Exercise exercise, out string msg);

			Assert.IsFalse(exFound, $"Invalid exercise ID should not be found.");
			Assert.IsTrue(exercise.id == 0, "Exercise ID is not 0.");
			Assert.IsTrue(string.IsNullOrEmpty(exercise.title), "Exercise title is not empty.");
			Assert.AreEqual(msg, $"Unable to find exercise with ID {exID}", "Message doesn't match on failure.");
		}
	}
}
