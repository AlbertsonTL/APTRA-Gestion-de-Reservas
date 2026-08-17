using Microsoft.AspNetCore.Mvc;

namespace APTRA_Gestion_de_Reservas.Controllers;

public class CajeroController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Cajero";
        return View();
    }
}
