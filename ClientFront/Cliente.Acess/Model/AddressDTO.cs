using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Acess.Model
{
    public class AddressDTO
    {
        public int ClientId { get; set; }
        public int Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }
}
