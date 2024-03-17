namespace Stossion.API.Templates

{
    public static class GetTemplates
	{
		public static string GetFileContent(string fileName,string fileType)
		{
            string filePath = Path.Combine(Environment.CurrentDirectory, "Templates",fileType,fileName);
			string fileContent = string.Empty;

			try
			{
				// Read the text from the file
				fileContent = System.IO.File.ReadAllText(filePath);
				return fileContent;
			}
			catch (IOException e)
			{
				// Handle file IO exception
				return null;
			}
		}
	}
}
