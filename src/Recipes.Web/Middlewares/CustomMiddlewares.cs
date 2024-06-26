﻿namespace Recipes.Middlewares;

public static class CustomMiddlewares
{
    public static async Task ExtendRequestDurationMiddleware(HttpContext context, RequestDelegate next)
    {
        if (new Random().Next(20) == 1)
        {
            await Task.Delay(new Random().Next(300, 500));
        }

        await next(context);
    }
}