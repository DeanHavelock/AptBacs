using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AptBacs.PaymentProcessor.Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessBacsPaymentService _processBacsPaymentService;

        public PaymentController(IProcessBacsPaymentService processBacsPaymentService)
        {
            _processBacsPaymentService = processBacsPaymentService;
        }

        /// <summary>
        /// Process and validates the file to be saved body: code: number, name: string, reference: string, amount: amount required: true produces: - application/json responses: '200': description: OK schema: $ref: '#/definitions/Submission' definitions: FileResponse: type: object required: filename totalLinesRead properties: filename: type: string totalLinesRead: type: number
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Payment
        ///     {
        ///        "fileName": sample.csv,
        ///        "totalLinesRead": "100",
        ///     }
        ///
        /// </remarks>
        /// <param name="fileName">text, example 'sample.csv'</param>
        /// <param name="code">numeric, example '123'</param>
        /// <param name="name">text, example 'abc'</param>
        /// <param name="reference">text, example 'xyz'</param>
        /// <param name="amount">decimal, example '50000.00'</param>
        /// <returns>A newly created PaymentId</returns>
        /// <response code="201">Returns the Id of the newly created item</response>
        /// <response code="400">Returns Bad Request Error</response>        
        // POST api/Payment    
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult Post(string fileName, int code, string name, string @reference, double amount)
        {
            var makePaymentCommand = new MakePaymentApplicationCommand() { FileName=fileName, PaymentRequestValueObjects = new List<PaymentRequestValueObject>() { new PaymentRequestValueObject() { Code=code,Name=name,Reference=@reference,Amount=amount } } };
            MakePayment(makePaymentCommand);
            return CreatedAtRoute("Post Payment", new { fileName, code, name, @reference, amount }, new { PaymentId=0 });
        }

        private void MakePayment(MakePaymentApplicationCommand makePaymentCommand)
        {
            _processBacsPaymentService.Pay(makePaymentCommand);
        }

    }
}