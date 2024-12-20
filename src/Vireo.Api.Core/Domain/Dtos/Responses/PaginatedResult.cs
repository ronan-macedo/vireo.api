﻿namespace Vireo.Api.Core.Domain.Dtos.Responses;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; private set; }

    public int TotalCount { get; private set; }

    public int PageSize { get; private set; }

    public int PageNumber { get; private set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public PaginatedResult(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        PageNumber = pageNumber;
    }
}
