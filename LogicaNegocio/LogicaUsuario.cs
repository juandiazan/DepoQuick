using Dominio;
using Repositorio;

namespace LogicaNegocio;

public class LogicaUsuario
{
    private IRepositorioFind<Usuario> _repositorioUsuarios;
    private LogicaSesion _logicaSesion;
    
    public LogicaUsuario(IRepositorioFind<Usuario> repositorioUsuarios, LogicaSesion logicaSesion) {
        _repositorioUsuarios = repositorioUsuarios;
        _logicaSesion = logicaSesion;
    }
    
    
    public Usuario Add(Usuario nuevoUsuario) {
        if (NoSePuedeAgregarNuevoUsuario(nuevoUsuario)) {
            throw new LogicaUsuarioException("Error al ingresar un nuevo usuario en el sistema");
        } else if (UsuarioConectadoEsCliente()) {
            throw new LogicaUsuarioException("Solo un administrador puede ingresar nuevos usuarios");
        } else if (ExisteUsuario(nuevoUsuario)) {
            throw new LogicaUsuarioException("Ese correo ya fue registrado");
        }

        if (!HayUsuariosRegistrados()) {
            nuevoUsuario.RolDeUsuario = Rol.Administrador;
        }
        else {
            nuevoUsuario.RolDeUsuario = Rol.Cliente;
        }
        
        _repositorioUsuarios.Add(nuevoUsuario);
        return nuevoUsuario;
    }
    public List<Usuario> GetAll() {
        return _repositorioUsuarios.GetAll();
    }
    public Usuario Find(string unCorreo) {
        Usuario usuarioRetorno = _repositorioUsuarios.Find(x => x.Email.Equals(unCorreo));
        
        if (usuarioRetorno is null) {
            throw new LogicaUsuarioException("El usuario no existe");
        }

        return usuarioRetorno;
    }
    public bool ExisteUsuario(Usuario unUsuario) {
        return GetAll().Any(usu => usu.Email.Equals(unUsuario.Email));
    }
    
    
    private bool NoSePuedeAgregarNuevoUsuario(Usuario nuevoUsuario) {
        return !HayUsuariosRegistrados() && EsCliente(nuevoUsuario) || HayAdministradorRegistrado() && !EsCliente(nuevoUsuario);
    }
    private bool HayUsuariosRegistrados() {
        return GetAll().Count != 0;
    }
    private bool EsCliente(Usuario unUsuario) {
        return unUsuario.RolDeUsuario == Rol.Cliente;
    }
    private bool HayAdministradorRegistrado() {
        return GetAll().Any(x => x.RolDeUsuario == Rol.Administrador);
    }
    private bool UsuarioConectadoEsCliente() {
        return _logicaSesion.HayUnClienteEnSesion();
    }
}