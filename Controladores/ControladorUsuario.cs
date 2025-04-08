using Dominio;
using LogicaNegocio;

namespace Controladores;

public class ControladorUsuario
{
    private LogicaUsuario _logicaUsuario;
    
    public ControladorUsuario(LogicaUsuario logicaUsuario)
    {
        _logicaUsuario = logicaUsuario;
    }
    
    public Usuario AgregarUsuario(Usuario unUsuario)
    {
        return _logicaUsuario.Add(unUsuario);
    }
    
    public Usuario BuscarUsuario(string unCorreo)
    {
        return _logicaUsuario.Find(unCorreo);
    }
    
    public List<Usuario> ObtenerTodosLosUsuarios()
    {
        return _logicaUsuario.GetAll();
    }
    
    public bool ExisteUsuario(Usuario unUsuario)
    {
        return _logicaUsuario.ExisteUsuario(unUsuario);
    }
}