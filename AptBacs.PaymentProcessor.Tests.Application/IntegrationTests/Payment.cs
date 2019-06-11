using System;
using System.Net.Http;
using Xunit;

namespace AptBacs.PaymentProcessor.Tests.Application.IntegrationTests
{
    public class Payment
    {
        [Fact]
        public void Given_ValidApiValues_When_SubmittedToPaymentApiForProcessing_Then_PaymentRecorded()
        {
            //Setup valid test request values
            string fileName = "sample.csv";
            int code = 1;
            string name = "abc";
            string reference = "xyz";
            double amount = 50000.00;

            //Setup endpoint test values
            string baseAddress = "https://localhost:44399";
            string endpointAddress = "/api/Payment";
            string addressPostValues = "?fileName=" + fileName + "&code=" + code + "&name=" + name + "&reference=" + reference + "&amount=" + amount;

            //Setup Http Request to Post Payment Test Endpoint:
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, baseAddress)
            {
                RequestUri = new Uri(baseAddress + endpointAddress + addressPostValues)
            };
            httpClient.SendAsync(requestMessage);

            //Check if expected values are saved to the Database:
            Assert.True(false);
        }


        [Fact]
        public void Given_ValidCsvFile_When_SubmittedToPaymentApiForProcessing_Then_PaymentRecorded()
        {
            //Create Csv File:

            //Post Csv File to Post Payment Endpoint:

            //Check if expected values are saved to the Database:
            Assert.True(false);
        }


    }
}
