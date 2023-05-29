using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using ValuationService.Models;

namespace ValuationService.Services
{
    public class PictureService
    {
        // Attributter
        private readonly ILogger<PictureService> _logger;
        private readonly IConfiguration _config;
        private readonly string imagepath;

        // Contructor
        public PictureService(ILogger<PictureService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            imagepath = _config["imagepath"];
        }

        // Tom contructor - kun til brug i test
        public PictureService()
        {

        }

        // Gemmer en liste af IFormFile-billeder på disken og returnerer en liste af stier til de gemte billeder
        public async Task<List<string>> SavePicture(List<IFormFile> files)
        {
            var paths = new List<string>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Generate a unique filename
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Construct the path to save the file
                    string filePath = Path.Combine(imagepath, fileName);

                    // Save the file to disk
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    paths.Add(filePath);
                }
            }
            return paths;
        }

        // Læser billeder fra disken baseret på filnavne og returnerer en liste af byte-arrays med billededata
        public List<byte[]> ReadPicture(List<string> filenames)
        {
            var images = new List<byte[]>();

            foreach (var filename in filenames)
            {
                string filePath = Path.Combine(imagepath, filename);

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    images.Add(fileBytes);
                }
            }
            return images;
        }

        // Læser og sletter billeder fra disken baseret på filnavne og returnerer en liste af byte-arrays med billededata
        public List<byte[]> ReadAndDeletePictures(List<string> filenames)
        {
            var images = new List<byte[]>();

            foreach (var filename in filenames)
            {
                string filePath = Path.Combine(imagepath, filename);

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    try
                    {
                        System.IO.File.Delete(filePath); // Delete the file after reading its bytes
                    }
                    catch (IOException ioExp)
                    {
                        Console.WriteLine($"Error occurred while deleting: {filePath}. Details: {ioExp.Message}");
                        throw; // Re-throw the exception to stop execution
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine($"An error occurred: {exp.Message}");
                        throw; // Re-throw the exception to stop execution
                    }
                    images.Add(fileBytes);
                }
            }
            return images;
        }
    }
}