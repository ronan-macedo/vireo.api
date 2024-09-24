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
            .OrderBy(client => client.LastServiceDate)
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
            .OrderBy(client => client.LastServiceDate)
            .ToListAsync();

        return new PaginatedResult<Client>(
            clients,
            totalCount,
            pageSize,
            pageNumber);
    }

    public override async Task<bool> UpdateAsync(Client entity)
    {
        Client? existingClient = await DbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Id == entity.Id);
        if (existingClient == null)
        {
            return false;
        }

        existingClient.FirstName = entity.FirstName;
        existingClient.LastName = entity.LastName;
        existingClient.Phone = entity.Phone;
        existingClient.Email = entity.Email;

        DbSet.Update(existingClient);
        await Context.SaveChangesAsync();
        return true;
    }
}
