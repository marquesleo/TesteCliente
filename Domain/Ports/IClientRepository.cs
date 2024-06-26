﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ports
{
    public interface IClientRepository: IRepository<Client>
    {
        Task<IEnumerable<Client>> GetClientByEmail(string email);   

    }
}
