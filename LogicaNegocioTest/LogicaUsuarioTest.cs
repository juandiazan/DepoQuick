using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaUsuarioTest
{
    private ContextoSql _contexto;
    
    private IRepositorioFind<Usuario> _repositorioUsuario;
    
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    
    [TestInitialize]
    public void SetUp() {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        
        _repositorioUsuario = new RepositorioUsuarioBaseDeDatos(_contexto);
        
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(_repositorioUsuario, _logicaSesion);
    }

    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AgregarAdministradorOk() {
        Usuario unUsuario = new Usuario()
        {
            Nombre = "unNombre",
            Apellido = "unApellido",
            Email = "unCorreo@gmail.com",
            Contrasena = "123456Aa#!",
            VerificacionContrasena = "123456Aa#!",
            RolDeUsuario = Rol.Administrador
        };
        
        Usuario nuevoUsuario = _logicaUsuario.Add(unUsuario);
        Assert.AreEqual(unUsuario.Email, nuevoUsuario.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void AgregarClienteSinQueHayaAdministradorRegistrado() {
        Usuario unCliente = new Usuario()
        {
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(unCliente);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void AgregarAdministradorCuandoYaHayUnoRegistrado() {
        Usuario primerAdministrador = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "unaContra123!",
            VerificacionContrasena = "unaContra123!",
            RolDeUsuario = Rol.Administrador
        };

        Usuario segundoAdministrador = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "unaContra123!",
            VerificacionContrasena = "unaContra123!",
            RolDeUsuario = Rol.Administrador
        };

        _logicaUsuario.Add(primerAdministrador);
        _logicaUsuario.Add(segundoAdministrador);
    }

    [TestMethod]
    public void AgregarUnClienteLuegoDeUnAdministradorOk() {
        Usuario unAdministrador = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "unaContra123!",
            VerificacionContrasena = "unaContra123!",
            RolDeUsuario = Rol.Administrador
        };

        Usuario unCliente = new Usuario()
        {
            Email = "unCorreo@otroCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "unaContra123!",
            VerificacionContrasena = "unaContra123!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(unAdministrador);
        _logicaUsuario.Add(unCliente);
    }

    [TestMethod]
    public void InicioSesionOk() {
        Usuario unUsuario = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "Contra123!!!",
            VerificacionContrasena = "Contra123!!!"
        };

        _logicaUsuario.Add(unUsuario);
        _logicaSesion.IniciarSesion(unUsuario, "Contra123!!!", _logicaUsuario.GetAll());
        Usuario usuarioIntentandoIniciarSesion = _logicaSesion.UsuarioEnSesionActual();
        Assert.AreEqual(unUsuario.Email, usuarioIntentandoIniciarSesion.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void InicioSesionConContrasenaDistintaALaDelUsuario() {
        Usuario unUsuario = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "Contra123!!!",
            VerificacionContrasena = "Contra123!!!"
        };

        _repositorioUsuario.Add(unUsuario);
        _logicaSesion.IniciarSesion(unUsuario, "ContraDistinta123#", _logicaUsuario.GetAll());
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void InicioSesionConUnCorreoNoRegistrado() {
        Usuario unUsuario = new Usuario()
        {
            Email = "unCorreoNoRegistrado@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!"
        };
        
        _logicaSesion.IniciarSesion(unUsuario, "UnaPass123!", _logicaUsuario.GetAll());
    }

    [TestMethod]
    public void CerrarSesionOk() {
        Usuario unUsuario = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!"
        };
        _repositorioUsuario.Add(unUsuario);
        _logicaSesion.IniciarSesion(unUsuario, "UnaPass123!", _logicaUsuario.GetAll());
        _logicaSesion.CerrarSesion();
        
        Assert.AreEqual(null, _logicaSesion.UsuarioEnSesionActual());
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void CreacionUsuarioCuandoUsuarioConectadoEsCliente() {
        Usuario unAdministrador = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };
        Usuario primerCliente = new Usuario()
        {
            Email = "unCorreo@unCorreoDeCliente",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        Usuario segundoCliente = new Usuario()
        {
            Email = "unCorreo@otroCorreoDeCliente",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(unAdministrador);
        _logicaSesion.IniciarSesion(unAdministrador, "UnaPass123!", _logicaUsuario.GetAll());
        _logicaUsuario.Add(primerCliente);
        _logicaSesion.CerrarSesion();
        
        _logicaSesion.IniciarSesion(primerCliente, "UnaPass123!", _logicaUsuario.GetAll());
        _logicaUsuario.Add(segundoCliente);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void RegistrarUnUsuarioConCorreoYaRegistrado() {
        Usuario unAdministrador = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };
        
        Usuario primerCliente = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        Usuario segundoCliente = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "ContraSeña321!",
            VerificacionContrasena = "ContraSeña321!",
            RolDeUsuario = Rol.Cliente
        };

        _logicaUsuario.Add(unAdministrador);
        _logicaUsuario.Add(primerCliente);
        _logicaUsuario.Add(segundoCliente);
    }
    
    [TestMethod]
    public void FindUsuarioExistenteRetornaUsuario()
    {
        Usuario usuario = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!"
        };
        _logicaUsuario.Add(usuario);
        _logicaSesion.IniciarSesion(usuario, "UnaPass123!", _logicaUsuario.GetAll());

        Usuario usuarioEncontrado = _logicaUsuario.Find(usuario.Email);

        Assert.IsNotNull(usuarioEncontrado);
        Assert.AreEqual(usuario.Email, usuarioEncontrado.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void FindUsuarioNoExistenteLanzaExcepcion()
    {
        _logicaUsuario.Find("nonExistingEmail@example.com");
    }
    
}