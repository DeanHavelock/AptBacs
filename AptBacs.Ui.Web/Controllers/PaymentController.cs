using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand.ValueObjects;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AptBacs.Ui.Web.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            //Setup endpoint values
            string baseAddress = "https://localhost:44399";
            string endpointAddress = "/api/Payment";

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var response = httpClient.GetAsync(endpointAddress).Result;

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            //Read response content result into string variable
            var strJson = response.Content.ReadAsStringAsync().Result;
            //Deserialize the string (Json format) to strongly typed object
            var paymentRequestsForUserReadModel = JsonConvert.DeserializeObject<PaymentRequestsForUserReadModel>(strJson);

            return View(paymentRequestsForUserReadModel);
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            List<KeyValuePair<string,string>> dataFileContentList = new List<KeyValuePair<string, string>>();
            List<string> dataFileNameList = new List<string>();
            foreach (var formFile in files)
            {
                var fileName = formFile.FileName.Split("\\").Last();
                if (formFile.Length > 0)
                {
                    dataFileContentList.Add(/*await*/new KeyValuePair<string, string>(fileName, ReadFileData(formFile)));
                }
            }

            foreach (var dataFileContent in dataFileContentList)
            {
                var dataFileContentRecords = CleanFileData(dataFileContent.Value);

                //validations
                ///ToDo: Add file data validation (not business logic) to make sure that there are no unexpected values in the serialized data, for example letters in the amount field throughout all records, throw exception if we have received incorrect data.

                MakePaymentApplicationCommand makePaymentApplicationCommand = new MakePaymentApplicationCommand();
                makePaymentApplicationCommand.FileName = dataFileContent.Key;

                foreach (var dataFileContentRecord in dataFileContentRecords)
                {
                    var dataFileContentRecordValues = dataFileContentRecord.Split(",");
                    string code = dataFileContentRecordValues[0].ToString();
                    string name = dataFileContentRecordValues[1].ToString();
                    string reference = dataFileContentRecordValues[2].ToString();
                    double amount = Convert.ToDouble(dataFileContentRecordValues[3]);
                    makePaymentApplicationCommand.PaymentRequestValueObjects.Add(new PaymentRequestValueObject(){ Code=code, Name=name, Reference=reference, Amount=amount });
                }

                //Setup endpoint values
                string baseAddress = "https://localhost:44399";
                string endpointAddress = "/api/Payment";
                
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseAddress)
                };
                await httpClient.PostAsJsonAsync(endpointAddress, makePaymentApplicationCommand);

            }
            //return Ok(new { count = files.Count, size, filePath });
            return RedirectToAction("Index");
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