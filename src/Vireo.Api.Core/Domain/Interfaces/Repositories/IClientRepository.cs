﻿using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Core.Domain.Interfaces.Repositories;

public interface IClientRepository : IBaseRepository<Client>
{
    Task<PaginatedResult<Client>> GetClientsAsync(int pageNumber, int pageSize);

    Task<PaginatedResult<Client>> SearchClientsAsync(string? name, string? lastName, int pageNumber, int pageSize);
}
