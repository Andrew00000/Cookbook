namespace Cookbook.API
{
    public static class ApiEndPoints
    {
        private const string ApiBase = "api";

        public static class Recipes
        {
            private const string Base = $"{ApiBase}/recipes";

            public const string Create = Base;
        }

    }
}
