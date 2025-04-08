using Dominio;

namespace Repositorio;

public class RepositorioUsuario : IRepositorioFind<Usuario>
{
    private List<Usuario> _listaUsuarios = new List<Usuario>();
    public Usuario Add(Usuario nuevoUsuario) {
        _listaUsuarios.Add(nuevoUsuario);
        return nuevoUsuario;
    }
    public List<Usuario> GetAll() {
        return _listaUsuarios;
    }
    public Usuario? Find(Func<Usuario, bool> filtro) {
        return _listaUsuarios.Where(filtro).FirstOrDefault();
    }
}