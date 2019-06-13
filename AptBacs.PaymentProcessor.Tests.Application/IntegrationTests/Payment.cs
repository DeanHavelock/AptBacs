using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand.ValueObjects;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xunit;

namespace AptBacs.PaymentProcessor.Tests.Application.IntegrationTests
{
    public class Payment
    {
        [Fact]
        public void Given_ValidApiValues_When_SubmittedToPaymentApiForProcessing_Then_PaymentRecorded()
        {
            //Setup Test Data:
            MakePaymentApplicationCommand makePaymentApplicationCommand = new MakePaymentApplicationCommand();
            makePaymentApplicationCommand.FileName = "sample.csv";

            for (int i= 0;i < 10;i++)
            {
                string code = "sampleCode"+i.ToString();
                string name = "sampleName" + i.ToString();
                string reference = "sampleReference" + i.ToString();
                double amount = Convert.ToDouble(i+10);
                makePaymentApplicationCommand.PaymentRequestValueObjects.Add(new PaymentRequestValueObject() { Code = code, Name = name, Reference = reference, Amount = amount });
            }

            //Setup endpoint values:
            string baseAddress = "https://localhost:44399";
            string endpointAddress = "/api/Payment";

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            //submit request:
            var content = new StringContent(JsonConvert.SerializeObject(makePaymentApplicationCommand), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(endpointAddress, content).Result;

            //check request:
            var response = httpClient.GetAsync(endpointAddress).Result;

            //Read response content result into string variable
            var strJson = response.Content.ReadAsStringAsync().Result;
            //Deserialize the string (Json format) to strongly typed object
            var paymentRequestsForUserReadModel = JsonConvert.DeserializeObject<PaymentRequestsForUserReadModel>(strJson);

            Assert.True(
                paymentRequestsForUserReadModel.PaymentRequestsForUser.First().SuccessfulPayments.Count() == 10
                );
        }

        [Fact]
        public void Given_TenValidApiValuesAndFiveInternalFailingValues_When_SubmittedToPaymentApiForProcessing_Then_PaymentTenSuccessfulPaymentsAndFiveFailedPaymentsRecorded()
        {
            //Setup Test Data:
            MakePaymentApplicationCommand makePaymentApplicationCommand = new MakePaymentApplicationCommand();
            makePaymentApplicationCommand.FileName = "sample.csv";

            //10 valid payments
            for (int i = 0; i < 10; i++)
            {
                string code = "sampleCode" + i.ToString();
                string name = "sampleName" + i.ToString();
                string reference = "sampleReference" + i.ToString();
                double amount = Convert.ToDouble(i + 10);
                makePaymentApplicationCommand.PaymentRequestValueObjects.Add(new PaymentRequestValueObject() { Code = code, Name = name, Reference = reference, Amount = amount });
            }

            // 5 invalid internal validation failing payment values
            for (int i = 0; i < 5; i++)
            {
                string code = "sampleCode" + i.ToString();
                string name = "sampleName" + i.ToString();
                string reference = "sampleReference" + i.ToString();
                double amount = Convert.ToDouble(0.13*i);
                makePaymentApplicationCommand.PaymentRequestValueObjects.Add(new PaymentRequestValueObject() { Code = code, Name = name, Reference = reference, Amount = amount });
            }

            //Setup endpoint values:
            string baseAddress = "https://localhost:44399";
            string endpointAddress = "/api/Payment";

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            //submit request:
            var content = new StringContent(JsonConvert.SerializeObject(makePaymentApplicationCommand), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(endpointAddress, content).Result;

            //check request:
            var response = httpClient.GetAsync(endpointAddress).Result;

            //Read response content result into string variable
            var strJson = response.Content.ReadAsStringAsync().Result;
            //Deserialize the string (Json format) to strongly typed object
            var paymentRequestsForUserReadModel = JsonConvert.DeserializeObject<PaymentRequestsForUserReadModel>(strJson);

            Assert.True(
                paymentRequestsForUserReadModel.PaymentRequestsForUser.First().SuccessfulPayments.Count() == 10 &&
                paymentRequestsForUserReadModel.PaymentRequestsForUser.First().FailedPayments.Count() == 5
                );
        }
    }
}
