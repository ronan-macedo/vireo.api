using Microsoft.EntityFrameworkCore;
using Vireo.Api.Core.Domain.Dtos.Responses;
using Vireo.Api.Core.Domain.Entities;
using Vireo.Api.Core.Domain.Interfaces.Repositories;
using Vireo.Api.Infrastructure.Data.Context;

namespace Vireo.Api.Infrastructure.Repositories;

public class ClientRepository : BaseRepository<Client>, IClientRepository
{
    public ClientRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<PaginatedResult<Client>> GetClientsAsync(int pageNumber, int pageSize)
    {
        int totalCount = await DbSet.AsNoTracking().CountAsync();
        List<Client> clients = await DbSet.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PaginatedResult<Client>(
            clients,
            totalCount,
            pageSize,
            pageNumber);
    }
}
