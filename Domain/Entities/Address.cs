using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {


        public Client Client { get; set; }
        public int ClientId {  get; set; }  
        public int Id { get; set; }

        public string Street { get; set; }

        public string  ZipCode { get; set; }
    }
}
