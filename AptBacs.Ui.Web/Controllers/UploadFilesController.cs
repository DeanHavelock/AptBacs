using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AptBacs.Ui.Web.Controllers
{
    public class UploadFilesController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            List<string> dataFileContentList = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    dataFileContentList.Add(/*await*/ReadFileData(formFile));
                }
            }

            foreach (var dataFileContent in dataFileContentList)
            {
                var dataFileContentRecords = CleanFileData(dataFileContent);

                //validations
                ///ToDo: Add file data validation (not business logic) to make sure that there are no unexpected values in the serialized data, for example letters in the amount field throughout all records, throw exception if we have received incorrect data.

                foreach (var dataFileContentRecord in dataFileContentRecords)
                {
                    var dataFileContentRecordValues = dataFileContentRecord.Split(",");
                    //send object model to api for payment processing (https)
                    //Setup request values from dataFileContent
                    string fileName = "sample.csv";
                    int code = Convert.ToInt32(dataFileContentRecordValues[0]);
                    string name = dataFileContentRecordValues[1].ToString();
                    string reference = dataFileContentRecordValues[2].ToString();
                    double amount = Convert.ToDouble(dataFileContentRecordValues[3]);

                    //Setup endpoint values
                    string baseAddress = "https://localhost:44399";
                    string endpointAddress = "/api/Payment";
                    string addressPostValues = "?fileName=" + fileName + "&code=" + code + "&name=" + name + "&reference=" + reference + "&amount=" + amount;

                    HttpClient httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(baseAddress)
                    };
                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseAddress)
                    {
                        RequestUri = new Uri(baseAddress + endpointAddress + addressPostValues)
                    };
                    await httpClient.SendAsync(requestMessage);
                }
            }
            return Ok(new { count = files.Count, size, filePath });
        }

        private List<string> CleanFileData(string dataFileContent)
        {
            //Cleanup data: split by /r/n, take first element away (titles)and last element (if no ',' values in last element), input values into object model.
            var dataFileContentRecords = dataFileContent.Split("\r\n").ToList();

            ///Ensure first record is only headings and cleanup:
            int isNumeric = 0;
            if (!Int32.TryParse(dataFileContentRecords[0].Split(",")[3], out isNumeric))
            {
                // value in Amount is not a number (it is a heading), remove the heading row.
                dataFileContentRecords.RemoveAt(0);
            };

            if (!dataFileContentRecords.ElementAt(dataFileContentRecords.Count - 1).Contains(','))
            {
                dataFileContentRecords.RemoveAt(dataFileContentRecords.Count - 1);
            }
            return dataFileContentRecords;
        }

        private string ReadFileData(IFormFile formFile)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            return result.ToString();
        }
    }
}