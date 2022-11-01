﻿using AppCitas.Service.Helpers;
using System.Text.Json;

namespace AppCitas.Service.Extensions;

public static class HttpExtension
{
    public static void AddPaginationHeader(this HttpResponse response, int currentPage,
        int itemsPerPage, int totalItems, int totalPages)
    {
        var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
        response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
    }
}