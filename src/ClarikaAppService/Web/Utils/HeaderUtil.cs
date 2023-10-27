using JHipsterNet.Core.Pagination;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClarikaAppService.Web.Utils;

public static class HeaderUtil
{
    private static readonly string APPLICATION_NAME = "clarikaAppServiceApp";

    public static IHeaderDictionary CreateAlert(string message, string param)
    {
        IHeaderDictionary headers = new HeaderDictionary();
        headers.Add($"X-{APPLICATION_NAME}-alert", message);
        headers.Add($"X-{APPLICATION_NAME}-params", param);
        return headers;
    }

    public static IHeaderDictionary CreateEntityCreationAlert(string entityName, string param)
    {
        return CreateAlert($"{APPLICATION_NAME}.{entityName}.created", param);
    }

    public static IHeaderDictionary CreateEntityUpdateAlert(string entityName, string param)
    {
        return CreateAlert($"{APPLICATION_NAME}.{entityName}.updated", param);
    }

    public static IHeaderDictionary CreateEntityDeletionAlert(string entityName, string param)
    {
        return CreateAlert($"{APPLICATION_NAME}.{entityName}.deleted", param);
    }

    public static IHeaderDictionary CreateFailureAlert(string entityName, string errorKey, string defaultMessage)
    {
        //log.error("Entity processing failed, {}", defaultMessage);
        IHeaderDictionary headers = new HeaderDictionary();
        headers.Add($"X-{APPLICATION_NAME}-error", $"error.{errorKey}");
        headers.Add($"X-{APPLICATION_NAME}-params", entityName);
        return headers;
    }
}

public static class PaginationUtil
{
    private const string _XTotalCountHeaderName = "X-Total-Count";
    private const string _XPaginationHeaderName = "X-Pagination";
    public static IHeaderDictionary GeneratePaginationHttpHeaders<T>(this IPage<T> page)
        where T : class
    {
        IHeaderDictionary headers = new HeaderDictionary();
        PageResponse pageDto = new PageResponse()
        {
            TotalCount = page.TotalElements,
            TotalPages = page.TotalPages,
            Page = page.Number,
            Size = page.Size,
            HasNext = page.HasNext,
            HasPrevious = page.HasPrevious,
            IsFirst = page.IsFirst,
            IsLast = page.IsLast
        };
        headers.Add(_XTotalCountHeaderName, page.TotalElements.ToString());
        headers.Add(_XPaginationHeaderName, JsonConvert.SerializeObject(pageDto));
        return headers;
    }
}