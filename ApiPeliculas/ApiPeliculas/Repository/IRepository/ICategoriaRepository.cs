using ApiPeliculas.Models;

namespace ApiPeliculas.Repository.IRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();

        Categoria GetCategoria(int CategoriaId);
        bool ExisteCategoria(string nombreCategoria);
        bool ExisteCategoria(int CategoriaId);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();


    }
}
