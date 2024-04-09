using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

      
        public string Email { get; set; }

        public List<Address> AddressList { get; set; }  


    }
}
