using Microsoft.AspNetCore.Mvc;

namespace AptBacs.PaymentProcessor.Application.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

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
        public ActionResult Post(int code, string name, string @reference, double amount)
        {
            return CreatedAtRoute("Post Payment", new { code, name, @reference, amount }, new { PaymentId=0 });
        }

    }
}