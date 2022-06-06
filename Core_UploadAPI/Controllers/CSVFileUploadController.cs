using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace Core_UploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSVFileUploadController : ControllerBase
    {
         private readonly IConfiguration _configuration;

        public CSVFileUploadController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("file/upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            try
            {
                string fileName = file.FileName;
                string connectionString = _configuration.GetValue<string>("ConnectionString");
                string contaiernName = _configuration.GetValue<string>("ContainerName");

                if (!CheckFileExtension(file))
                {
                    return BadRequest($"The File does not have an extension or it is not image. " +
                        $"The Expected extension is .csv");
                }

                if (!CheckFileSize(file))
                {
                    return BadRequest($"The size of file is more than 10 mb, " +
                        $"please make sure that the file size must be less than 10 mb");
                }

                // Create a BLOB Container if not exist
                BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, contaiernName);
                await blobContainerClient.CreateIfNotExistsAsync();

                // Get the reference of the BLOB
                BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
              
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                string finalPath = Path.Combine(folder,fileName);

                using (var fs = new FileStream(finalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                // Upload file
                await blobClient.UploadAsync(finalPath, true);

                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {
                return Ok($"File Uploaded Failed {ex.Message}");
            }
        }

        /// The file extension must be csv

        private bool CheckFileExtension(IFormFile file)
        {
            string[] extensions = new string[] { "csv" };

            var fileNameExtension = file.FileName.Split(".")[1];
            if (string.IsNullOrEmpty(fileNameExtension) ||
                !extensions.Contains(fileNameExtension))
            {
                return false;
            }

            return true;
        }

        /// Check the file size, it must be less than 10 mb

        private bool CheckFileSize(IFormFile file)
        {
            if (file.Length > 1e+7)
            {
                return false;
            }
            return true;
        }
    }
}

