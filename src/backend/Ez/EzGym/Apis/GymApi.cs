using Microsoft.AspNetCore.Builder;

namespace EzGym.Apis
{
    public static class GymApi
    {
        public static WebApplication UseEzGymGymsApi(this WebApplication app)
        {

            app.MapPost("/gyms/users", () =>
            {

            });

            app.MapPost("/gyms/plans", () =>
            {

            });

            app.MapGet("/gyms/{id}/wallet", () =>
            {

            });

            return app;
        }
    }
}
