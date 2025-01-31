using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests
{
	[TestClass]
	public class SearchByNameTests
	{
		[TestMethod]
		public void UserIDFound()
		{
			var dataSrc   = MockData.GetDataSource();
			var userFirst = MockData.TestUserFirst_B;
			var userLast  = MockData.TestUserLast_B;
			var userFound = dataSrc.TryGetUserIDByName(userFirst, out int userID, out string firstLast, out string msg);

			Assert.IsTrue(userFound, $"Partial user name {userFirst} did not find any results.");
			Assert.IsTrue(userID == 5678, "User ID was not expected value.");
			Assert.AreEqual(firstLast, $"{userFirst} {userLast}", "User output name was not expected value.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == true, "Message was not empty on success.");
		}

		[TestMethod]
		public void UserIDNotFound()
		{
			var dataSrc   = MockData.GetDataSource();
			var userName  = "AAAA";
			var userFound = dataSrc.TryGetUserIDByName(userName, out int userID, out string firstLast, out string msg);

			Assert.IsFalse(userFound, "User should not be found.");
			Assert.AreEqual(userID, -1, "User ID should be -1.");
			Assert.IsTrue(string.IsNullOrEmpty(firstLast), "Output user name is not empty.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == false, "Failure message is empty.");
		}

		[TestMethod]
		public void MultipleUsersFound()
		{
			var dataSrc   = MockData.GetDataSource();
			var userName  = "Jon";
			var userFound = dataSrc.TryGetUserIDByName(userName, out int userID, out string firstLast, out string msg);

			Assert.IsFalse(userFound, "Multiple user matches should not return success.");
			Assert.AreEqual(userID, -1, "User ID should be -1.");
			Assert.IsTrue(string.IsNullOrEmpty(firstLast), "Output user name is not empty.");
			Assert.IsTrue(msg.Contains("Multiple matches found"), "Failure message contains unexpected result.");
		}

		[TestMethod]
		public void ExerciseIDFound()
		{
			var dataSrc = MockData.GetDataSource();
			var exName  = "Leg";
			var exFound = dataSrc.TryGetExerciseIDByName(exName, out int exID, out string fullName, out string msg);

			Assert.IsTrue(exFound, $"Partial exercise {exName} did not find any results.");
			Assert.IsTrue(exID == 111, "Exercise ID was not expected value.");
			Assert.AreEqual(fullName, MockData.TestExerciseTitle_B, "Exercise title was not expected value.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == true, "Message was not empty on success.");
		}

		[TestMethod]
		public void ExerciseNotFound()
		{
			var dataSrc = MockData.GetDataSource();
			var exName  = "AAAA";
			var exFound = dataSrc.TryGetExerciseIDByName(exName, out int exID, out string fullName, out string msg);

			Assert.IsFalse(exFound, "Exercise should not be found.");
			Assert.AreEqual(exID, -1, "Exercise ID should be -1.");
			Assert.IsTrue(string.IsNullOrEmpty(fullName), "Output exercise name is not empty.");
			Assert.IsTrue(string.IsNullOrEmpty(msg) == false, "Failure message is empty.");
		}

		[TestMethod]
		public void MultipleExercisesFound()
		{
			var dataSrc = MockData.GetDataSource();
			var exID    = "Press";
			var exFound = dataSrc.TryGetExerciseIDByName(exID, out int userID, out string fullName, out string msg);

			Assert.IsFalse(exFound, "Multiple exercise matches should not return success");
			Assert.AreEqual(userID, -1, "Exercise ID should be -1.");
			Assert.IsTrue(string.IsNullOrEmpty(fullName), "Output exercise name is not empty.");
			Assert.IsTrue(msg.Contains("Multiple matches found"), "Failure message contains unexpected result.");
		}
	}
}
