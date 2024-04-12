using dot7.razor.crudsample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace dot7.razor.crudsample.Pages.Employee;

public class EmployeeCreate : PageModel
{
    private Cliente.Acess.ClientAPIService _access;
    private Cliente.Acess.ClientAPIService Access
    {
        get
        {
            if (_access == null)
                _access = new Cliente.Acess.ClientAPIService();
            return _access;
        }
    }

    public EmployeeCreate()
    {
       
    }
    [BindProperty]
    public dot7.razor.crudsample.Data.Entities.Client NewClient { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var clientDTO = new Application.DTO.ClientDTO()
        {
            Name = NewClient.Name,
            Email = NewClient.Email,
            Id = NewClient.Id,
           
            
        };
        if (NewClient.Addresses != null && NewClient.Addresses.Any())
        {
            clientDTO.Address = new List<Application.DTO.AddressDTO>();
            foreach (var address in NewClient.Addresses)
            {
                if (!string.IsNullOrEmpty(address.Street) && !string.IsNullOrEmpty(address.ZipCode))
                {
                    var addressDTO = new Application.DTO.AddressDTO();
                    addressDTO.Street = address.Street;
                    addressDTO.ZipCode = address.ZipCode;
                     clientDTO.Address.Add(addressDTO);
                }
            }

        }
       await Access.Post(clientDTO);
      /*  _myWorldDbContext.Employee.Add(NewClient);
        await _myWorldDbContext.SaveChangesAsync();*/
        return Redirect("index");
    }
}