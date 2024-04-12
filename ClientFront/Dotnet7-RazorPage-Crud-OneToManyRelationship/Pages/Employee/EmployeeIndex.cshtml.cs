using dot7.razor.crudsample.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace dot7.razor.crudsample.Pages.Employee;

public class EmployeeIndex : PageModel
{
    
    public EmployeeIndex()
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

    public List<dot7.razor.crudsample.Data.Entities.Client> AllClient { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var Clients = await Access.GetAll();

        if (Clients  != null && Clients.Any())
        {
            AllClient = new List<Data.Entities.Client>();
            foreach (var clientDTO in Clients)
            {
                var client = Data.Entities.Client.MapToEntity(clientDTO);
                AllClient.Add(client);
            }
        }
        else
        {
            AllClient = new List<Data.Entities.Client>();
        }


        return Page();
    }
}