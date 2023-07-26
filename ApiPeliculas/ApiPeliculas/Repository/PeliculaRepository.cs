using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ApiPeliculas.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly AplicationDbContext _bd;

        public PeliculaRepository(AplicationDbContext _bd)
        {
            this._bd = _bd;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Peliculas.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);
            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;
            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _bd.Peliculas.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombrePelicula)
        {
            return _bd.Peliculas.Any(category => category.Nombre.ToLower().Trim() == nombrePelicula.ToLower().Trim());
        }

        public bool ExistePelicula(int peliculaId)
        {
            return _bd.Peliculas.Any(pelicula => pelicula.Id == peliculaId);
        }

        public Pelicula GetPelicula(int peliculaId)
        {
           return _bd.Peliculas.FirstOrDefault(pelicula => pelicula.Id == peliculaId);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(pelicula => pelicula.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _bd.Peliculas.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == categoriaId).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
