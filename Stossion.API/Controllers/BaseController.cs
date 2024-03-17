using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stossion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private static class GetTemplates
        {
            private static string GetFileContent(string fileName, string fileType)
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, "Templates", fileType, fileName);
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
}
