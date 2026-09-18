using WebStudio.Entities.Interaction;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class ResponseHelper
    {
        public static Response<T> CreateErrorResponse<T>(string errorMessage)
        {
            var response = new Response<T>();
            response.AddError(errorMessage);
            return response;
        }

        public static Response<T> CreateSuccessResponse<T>()
        {
            var response = new Response<T>();
            response.AddSuccess("Success!");
            return response;
        }
    }
}
