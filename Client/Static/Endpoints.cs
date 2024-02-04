namespace fooddelivary.Client.Static
{
    public static class Endpoints
    {
         private static readonly string BaseUrl = "https://localhost:44387/";

            public static string ProductsEndpoint = $"{BaseUrl}api/products";
            public static string OrdersEndpoint = $"{BaseUrl}api/orders";
            public static string UserTypeEndpoint = $"{BaseUrl}usertype";
        

    }
}
