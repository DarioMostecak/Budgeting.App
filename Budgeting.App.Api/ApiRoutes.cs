namespace Budgeting.App.Api
{
    public class ApiRoutes
    {
        public const string BaseRoute = "api/v{version:apiVersion}/";

        public static class Category
        {
            public const string CategoryBaseRoute = BaseRoute + "categories";
            public const string IdRoute = "{categoryId}";
        }
    }
}
