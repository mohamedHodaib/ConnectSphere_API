using System;
using System.Threading;
using System.Threading.Tasks;
using ConnectSphere.Application.Common.Interfaces;
using ConnectSphere.Application.Common.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

public class CachingBehavior<TRequest, TResponse>(
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IMemoryCache cache)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheable
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();

        // TryGetValue replaces GetAsync. It fetches the Result object.
        if (cache.TryGetValue(request.CacheKey, out TResponse? cachedResponse) && cachedResponse != null)
        {
            logger.LogInformation("Fetched from cache with key: {CacheKey}", request.CacheKey);
            return cachedResponse;
        }
        // Fetch from DB via the next MediatR handler
        var response = await next();

        // Don't cache null or failure results
        if (response is null || response is IResult { IsSuccess: false })
        {
            logger.LogInformation(
                "Response not cached (null or failure) for key: {CacheKey}", request.CacheKey);
            return response;
        }

        if (response != null)
        {
            var options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes))
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes));

            // Set saves the object reference directly. 
            cache.Set(request.CacheKey, response, options);
            logger.LogInformation("Added to cache with key: {CacheKey}", request.CacheKey);
        }

        return response;
    }
}