using Domain.Entities;
using Domain.Ports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ClientRepository : IClientRepository
    {

        private readonly ClientDBContext _clientDbContext;
        public ClientRepository(ClientDBContext clientDbContext)
        {
            _clientDbContext = clientDbContext;
        }
        public async Task DeleteById(int id)
        {
            var client = _clientDbContext.Clients.Where(c => c.Id == id).FirstOrDefault();
            _clientDbContext.Clients.Remove(client);
            await _clientDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _clientDbContext.Clients.Include(r => r.AddressList).ToListAsync();
        }

        public async Task<Client> GetById(int id)
        {
            return await _clientDbContext.Clients
               .Where(g => g.Id == id).Include(r=> r.AddressList).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetClientByEmail(string email)
        {
            return await _clientDbContext.Clients.Where(c => c.Email.ToLower() == email.ToLower()).ToListAsync();
        }

        public async Task<Client> Save(Client entity)
        {
            _clientDbContext.Clients.Add(entity);
            await _clientDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(Client entity)
        {
            _clientDbContext.Clients.Update(entity);
            await _clientDbContext.SaveChangesAsync();
            
        }
    }
}
