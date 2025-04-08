using Dominio;

namespace Repositorio;

public class RepositorioUsuarioBaseDeDatos : IRepositorioFind<Usuario>
{
    private ContextoSql _contexto;

    public RepositorioUsuarioBaseDeDatos(ContextoSql contexto) {
        _contexto = contexto;
    }
    
    public Usuario Add(Usuario unElemento) {
        _contexto.Usuarios.Add(unElemento);
        _contexto.SaveChanges();
        return unElemento;
    }

    public List<Usuario> GetAll() {
        return _contexto.Usuarios.ToList();
    }

    public Usuario? Find(Func<Usuario, bool> unFiltro) {
        return _contexto.Usuarios.FirstOrDefault(unFiltro);
    }
}