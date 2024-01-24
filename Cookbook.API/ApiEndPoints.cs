namespace Cookbook.API
{
    public static class ApiEndPoints
    {
        private const string ApiBase = "api";

        public static class Recipes
        {
            private const string Base = $"{ApiBase}/recipes";

            public const string Create = Base;
            public const string Get = $"{Base}/{{idOrSlug}}";
            public const string GetAll = Base;
            public const string GetAllTitles = $"{Base}/titles";
            public const string GetAllTitlesWithTag = $"{GetAllTitles}/tags/{{tag}}";
            public const string Update = $"{Base}/{{idOrSlug}}";
            public const string Delete = $"{Base}/{{idOrSlug}}";
        }
    }
}
