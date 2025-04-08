using Dominio;

namespace LogicaNegocio;

public class LogicaSesion
{
    private Usuario? _usuarioEnSesion;

    
    public void IniciarSesion(Usuario unUsuario, string unaContrasena, List<Usuario> listaUsuarios) {
        if (!EsNulo(unUsuario) && ExisteUsuario(listaUsuarios, unUsuario.Email) && UsuarioIngresoContraseñaCorrecta(unUsuario, unaContrasena)) {
            _usuarioEnSesion = unUsuario;
        }
        else {
            throw new LogicaUsuarioException("El usuario ingresó la contraseñá incorrecta ó el usuario no existe");
        }
    }
    public Usuario? UsuarioEnSesionActual() {
        return _usuarioEnSesion;
    }
    public bool HayUnClienteEnSesion() {
        return HayUnaSesionIniciada() && _usuarioEnSesion.RolDeUsuario.Equals(Rol.Cliente);
    }
    public bool HayUnaSesionIniciada() {
        return _usuarioEnSesion is not null;
    }
    public void CerrarSesion() {
        _usuarioEnSesion = null;
    }

    
    private bool ExisteUsuario(List<Usuario> listaUsuarios, string correo) {
        return listaUsuarios.Any(x => x.Email == correo);
    }
    private bool EsNulo(Usuario? unUsuario) {
        return unUsuario is null;
    }
    private bool UsuarioIngresoContraseñaCorrecta(Usuario unUsuario, string unaContrasena) {
        return unUsuario.Contrasena.Equals(unaContrasena);
    }
}