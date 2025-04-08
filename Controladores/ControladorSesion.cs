using Dominio;
using LogicaNegocio;

namespace Controladores;

public class ControladorSesion
{
    private LogicaSesion _logicaSesion;

    public ControladorSesion(LogicaSesion logicaSesion)
    {
        _logicaSesion = logicaSesion;
    }
    
    public void IniciarSesion(Usuario unUsuario, string unaContrasena, List<Usuario> listaUsuarios)
    {
        _logicaSesion.IniciarSesion(unUsuario, unaContrasena, listaUsuarios);
    }

    public Usuario? ObtenerUsuarioEnSesion()
    {
        return _logicaSesion.UsuarioEnSesionActual();
    }
    
    public bool HayUnClienteEnSesion()
    {
        return _logicaSesion.HayUnClienteEnSesion();
    }

    public bool HayUnaSesionIniciada()
    {
        return _logicaSesion.HayUnaSesionIniciada();
    }
    
    public void CerrarSesion()
    {
        _logicaSesion.CerrarSesion();
    }
}