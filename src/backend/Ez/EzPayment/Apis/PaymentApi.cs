using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EzPayment.Infra.Repository;
using EzPayment.Payments;
using EzPayment.Webhooks.Payments;

namespace EzPayment.Apis
{
    public static class PaymentApi
    {
        public static WebApplication UseEzPaymentApi(this WebApplication app)
        {
            app.UsePaymentWebHooks();

            app.MapGet("/payments/{id}",
                [Authorize] (
                [FromServices] IPaymentRepository repository,
                string id) =>
                {
                    var payment = repository.QueryOne<Payment>(p => p.Id == id);
                    return Results.Ok(@payment);
                });


            return app;
        }
    }
}
