using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using System.ComponentModel;

namespace ApiPeliculas.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AplicationDbContext _bd;

        public CategoriaRepository(AplicationDbContext _bd)
        {
            this._bd = _bd;
        }
        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categoria.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categoria.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _bd.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombreCategoria)
        {
            return _bd.Categoria.Any(category => category.Nombre.ToLower().Trim() == nombreCategoria.ToLower().Trim());
        }

        public bool ExisteCategoria(int CategoriaId)
        {
            return _bd.Categoria.Any(category => category.Id == CategoriaId);
        }

        public Categoria GetCategoria(int CategoriaId)
        {
           return _bd.Categoria.FirstOrDefault(category => category.Id == CategoriaId);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categoria.OrderBy(category => category.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
