using System.Text;

namespace trainingpeaks
{
	public static class FileWriter
	{
		/// <summary>
		/// Writes a JSON file to the specified path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="fileContents"></param>
		public static void WriteJson(string filePath, string fileContents)
		{
			ConsoleKeyInfo keyInfo;

			if(File.Exists(filePath))
			{
				do
				{
					Console.WriteLine($"{filePath} already exists. Overwrite? y/n");
					keyInfo = Console.ReadKey();
					Console.Clear();
					if (keyInfo.Key == ConsoleKey.N)
					{
						Console.WriteLine("File write cancelled.\n");
						return;
					}
				}
				while (keyInfo.Key != ConsoleKey.Y);
			}

			File.WriteAllText(filePath, fileContents, Encoding.UTF8);
			Console.WriteLine($"File saved at path: {filePath}");
		}
	}
}
