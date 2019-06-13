using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationCommand;
using AptBacs.PaymentProcessor.Domain.ApplicationInterfaces.ApplicationReadModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AptBacs.PaymentProcessor.Application.Api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessBacsPaymentService _processBacsPaymentService;
        private readonly IBacsPaymentQueryService _bacsPaymentQueryService;

        public PaymentController(IProcessBacsPaymentService processBacsPaymentService, IBacsPaymentQueryService bacsPaymentQueryService)
        {
            _processBacsPaymentService = processBacsPaymentService;
            _bacsPaymentQueryService = bacsPaymentQueryService;
        }
  
        //[Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult Post([FromBody]MakePaymentApplicationCommand makePaymentApplicationCommand)
        {
            MakePayment(makePaymentApplicationCommand);
            return Ok();
        }

        private void MakePayment(MakePaymentApplicationCommand makePaymentCommand)
        {
            _processBacsPaymentService.Pay(makePaymentCommand);
        }

        //[Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [HttpGet]
        public JsonResult Get(int userId)
        {
            var paymentRequestsForUserReadModel = GetPaymentsForUser(userId);
            return new JsonResult(paymentRequestsForUserReadModel);
        }

        private PaymentRequestsForUserReadModel GetPaymentsForUser(int userId)
        {
            return _bacsPaymentQueryService.Get(userId);
        }
    }
}