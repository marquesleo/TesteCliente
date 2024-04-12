using dot7.razor.crudsample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dot7.razor.crudsample.Pages.Employee;

public class EmployeeDetails : PageModel
{
   
    public EmployeeDetails()
    {
        
    }

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

    public dot7.razor.crudsample.Data.Entities.Client Client { get; set; }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var clientDTO = await Access.GetCliente(id); 
        if (clientDTO != null)
        {
            Client  = Data.Entities.Client.MapToEntity(clientDTO);
        }
        return Page();
    }
}