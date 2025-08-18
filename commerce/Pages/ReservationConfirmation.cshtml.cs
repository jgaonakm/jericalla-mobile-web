using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;


public class ReservationConfirmationModel : PageModel
{
    [BindProperty]
    [Required, EmailAddress]
    public string? Email { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPostSubscribe()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // TODO: Save subscription to database or external newsletter service
        TempData["SuccessMessage"] = "¡Gracias por suscribirte! Pronto recibirás nuestras novedades.";
        return RedirectToPage("./ReservationConfirmation");
    }
}
