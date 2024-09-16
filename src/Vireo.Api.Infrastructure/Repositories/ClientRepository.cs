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
        List<Client> clients = await DbSet.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Client>(
            clients,
            totalCount,
            pageSize,
            pageNumber);
    }

    public async Task<PaginatedResult<Client>> SearchClientsAsync(
        string? name,
        string? lastName,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Client> query = DbSet.AsNoTracking();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(client => client.FirstName.Contains(name));
        }

        if (!string.IsNullOrEmpty(lastName))
        {
            query = query.Where(client => client.LastName.Contains(lastName));
        }

        int totalCount = await query.CountAsync();
        List<Client> clients = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<Client>(
            clients,
            totalCount,
            pageSize,
            pageNumber);
    }

    public override async Task<bool> UpdateAsync(Client entity)
    {
        return await DbSet.Where(client => client.Id == entity.Id)
            .ExecuteUpdateAsync(client =>
            client.SetProperty(prop => prop.FirstName, entity.FirstName)
                  .SetProperty(prop => prop.LastName, entity.LastName)
                  .SetProperty(prop => prop.Phone, entity.Phone)
                  .SetProperty(prop => prop.Email, entity.Email)
            ) > 0;
    }
}
