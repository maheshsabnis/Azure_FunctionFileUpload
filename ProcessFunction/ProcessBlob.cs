using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using ProcessFunction.Models;

namespace ProcessFunction
{
    public class ProcessBlob
    {
        [FunctionName("BlobProcessor")]
        public async Task Run([BlobTrigger("csvcontainer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

                log.LogInformation($"CSV File");
                if(myBlob.Length > 0)
                {
                    using (var reader = new StreamReader(myBlob))
                    {
                        var headers = await reader.ReadLineAsync();

                        var startLineNumber = 1;
                        var currentLine = await reader.ReadLineAsync();
                        while (currentLine != null)
                        {
                            currentLine = await reader.ReadLineAsync();
                            await AddLineToTable(currentLine, log);
                            startLineNumber++;
                        }

                    }
            }
        }


        private static async Task AddLineToTable(string line, ILogger log)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                log.LogInformation("The Current Record is empty");
                return;
            }

            var columns = line.Split(',');
            var person = new PersonInfo()
            {
              BusinessEntityId = Convert.ToInt32(columns[0]),
              PersonType = columns[1],
              NameStyle = Convert.ToInt32(columns[2]),
              FirstName = columns[3],
              LastName = columns[4],
              EmailPromotion = Convert.ToInt32(columns[5]) 
            };

            var context = new PersonDbContext();
            await context.PersnInfos.AddAsync(person);
            await context.SaveChangesAsync();
        }
    }
}
