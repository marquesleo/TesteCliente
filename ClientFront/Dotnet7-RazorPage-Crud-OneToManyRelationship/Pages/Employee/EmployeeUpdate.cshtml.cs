using dot7.razor.crudsample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dot7.razor.crudsample.Pages.Employee;

public class EmployeeUpdate : PageModel
{
    
    public EmployeeUpdate()
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

    [BindProperty]
    public dot7.razor.crudsample.Data.Entities.Client ClientToUpdate { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {

        var clientDTO = await Access.GetCliente(id);
        if (clientDTO != null)
        {
            ClientToUpdate = Data.Entities.Client.MapToEntity(clientDTO);
        }
         
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {

        var clientDTO = new Application.DTO.ClientDTO();
        clientDTO = Data.Entities.Client.MapToDTO(ClientToUpdate);

        await Access.Post(clientDTO); 

       
        return Redirect("index");
    }
}