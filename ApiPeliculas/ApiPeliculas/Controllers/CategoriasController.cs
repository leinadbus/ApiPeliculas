using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]  //Esta es una opción común
    [Route("api/Categorias")]      //Esta opción es mejor si en un futuro cambiamos el nombre de la clase

    public class CategoriasController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
