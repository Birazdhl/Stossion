using Stossion.Helpers.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Helpers.ImageHelper
{
    public static class Image
    {

        public static string GetImage(string folder, string name, string type = "PNG")
        {
            try
            {
                // Combine the specified location with the root directory
                string imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", folder, name + "." + type);

                // Check if the image file exists
                if (!File.Exists(imagePath))
                {
                    imagePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "user", "noImage.png");
                }

                // Read the image file as a byte array
                byte[] imageBytes = File.ReadAllBytes(imagePath);

                // Convert the byte array to a base64 string
                string base64String = Convert.ToBase64String(imageBytes);

                // Return the base64-encoded image string
                return base64String;
            }
            catch (Exception ex)
            {
                // Handle any exceptions here (e.g., log the error)
                Console.WriteLine($"Error retrieving image: {ex.Message}");
                return string.Empty; // Or throw an exception, depending on your requirements
            }
        }

        public static void SaveImage(string folder, string imageString, string? name = null, string? extention = null)
        {
            try
            {
                extention = extention ?? ImageType.png.ToString();
                // Create the directory if it doesn't exist
                string folderPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", folder);
                Directory.CreateDirectory(folderPath);

                // Generate a unique file name
                string fileName = name + "." + extention; // You may adjust the extension based on the image type

                // Combine folder path with the file name
                string imagePath = Path.Combine(folderPath, fileName);

                // Convert base64 string to byte array
                byte[] imageBytes = Convert.FromBase64String(imageString);

                // Write the byte array to a file
                File.WriteAllBytes(imagePath, imageBytes);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
