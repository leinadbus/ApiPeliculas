using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface IPeliculaRepository
    {
        ICollection<Pelicula> GetPeliculas();

        Pelicula GetPelicula(int PeliculaId);
        bool ExistePelicula(string nombrePelicula);
        bool ExistePelicula(int PeliculaId);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);

        //Métodos de busqueda de peliculas en categorias y busqueda por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);
        ICollection<Pelicula> BuscarPelicula(string nombre);


        bool Guardar();


    }
}
