using System.Text;

namespace trainingpeaks
{
	public static class FileWriter
	{
		/// <summary>
		/// Writes a JSON file to the specified path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="fileName"></param>
		/// <param name="fileContents"></param>
		public static void WriteJson(string filePath, string fileName, string fileContents)
		{
			ConsoleKeyInfo keyInfo;

			var fullPath = Path.Combine(filePath,fileName);

			if(File.Exists(fullPath))
			{
				do
				{
					Console.WriteLine($"File {fileName} exists at path. Overwrite? y/n");
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

			File.WriteAllText(fullPath, fileContents, Encoding.UTF8);
			Console.WriteLine($"File saved at path: {fullPath}");
		}
	}
}
