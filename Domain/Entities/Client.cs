using Domain.Ports;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class Client
    {       
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public List<Address> AddressList { get; set; }  

        private async Task Validate(IClientRepository repository)
        {
            if (!string.IsNullOrWhiteSpace(this.Email)) {
                var lstClientesEmail = await repository.GetClientByEmail(this.Email);
                if (this.Id == 0) {
                    if (lstClientesEmail == null || lstClientesEmail.Any())
                    {
                        throw new Exceptions.EmailDuplicateException();
                    }
                }

                if (!this.IsValidEmail(this.Email))
                    throw new Exceptions.InvalidEmailException();

            }

            if (this.AddressList != null && this.AddressList.Any()){

                var enderecosRepetidos = this.AddressList
                    .GroupBy(e => new { e.Street, e.ZipCode })
                    .Where(g => g.Count() > 1)
                     .Select(g => g.Key);

                if (enderecosRepetidos.Any())
                {
                    throw new Exceptions.AddressDuplicateException();
                }
            }

        }

       private bool IsValidEmail(string email)
        {
            // Define a expressão regular para validar o e-mail
            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            // Cria um objeto Regex com o padrão especificado
            Regex regex = new Regex(pattern);

            // Verifica se o e-mail corresponde ao padrão
            return regex.IsMatch(email);
        }

        public async Task<int> Save(IClientRepository repository)
        {
            await Validate(repository);
            Client client = null;
            if (this.Id == 0)
            {
                client = await repository.Save(this);
            }
            else
            {
                await repository.Update(this);
                client = this;
            }
            return client.Id;
        }


        public async Task Excluir(IClientRepository repository)
        {
            await repository.DeleteById(this.Id);
        }
    }
}
