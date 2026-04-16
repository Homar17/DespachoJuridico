using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IConfiguration _config;
        public PagosController(IConfiguration config)
        {
            _config = config;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        [HttpPost("crear-sesion")]
        public ActionResult CrearSesionCheckout()
        {
            // La URL donde está corriendo tu proyecto de Vue
            var dominioFrontend = "http://localhost:5173";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            // Stripe lee en centavos. 50000 = $500.00 MXN
                            UnitAmount = 20000, 
                            Currency = "mxn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Asesoría Jurídica Inicial",
                                Description = "Reserva de fecha y hora para consulta legal."
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                // A dónde redirigir si paga con éxito o si cancela
                SuccessUrl = dominioFrontend + "/?pago=exitoso",
                CancelUrl = dominioFrontend + "/?pago=cancelado",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Le devolvemos la URL de cobro segura a tu frontend de Vue
            return Ok(new { url = session.Url });
        }
    }
}