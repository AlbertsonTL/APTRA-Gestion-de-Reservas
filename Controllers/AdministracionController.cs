using Microsoft.AspNetCore.Mvc;

namespace APTRA_Gestion_de_Reservas.Controllers;

public class AdministracionController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Administración";
        return View();
    }

    public IActionResult Tickets()
    {
        ViewData["Title"] = "Tickets";
        return View();
    }
}
