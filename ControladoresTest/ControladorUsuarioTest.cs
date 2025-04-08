using Controladores;
using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace ControladoresTest;

[TestClass]
public class ControladorUsuarioTest
{
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    private ControladorUsuario _controladorUsuario;
    private ContextoSql _contexto;

    [TestInitialize]
    public void Setup()
    {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        RepositorioUsuarioBaseDeDatos repositorioUsuarios = new RepositorioUsuarioBaseDeDatos(_contexto);
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(repositorioUsuarios, _logicaSesion);
        _controladorUsuario = new ControladorUsuario(_logicaUsuario);
    }
    
    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    

    [TestMethod]
    public void AgregarUsuarioUsuarioNuevoUsuarioAgregadoTest()
    {
        Usuario nuevoUsuario = new Usuario
        {
            Nombre = "OtroNombre", Apellido = "unApellidoDos", Email = "unCorreoDos@gmail.com",
            Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!"
        };
        Usuario resultado = _controladorUsuario.AgregarUsuario(nuevoUsuario);

        Assert.IsNotNull(_controladorUsuario.BuscarUsuario(nuevoUsuario.Email));
        Assert.AreEqual<string>(nuevoUsuario.Email, resultado.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void AgregarUsuarioUsuarioExistenteExcepcionTest()
    {
        Usuario nuevoUsuario = new Usuario
        {
            Nombre = "OtroNombre", Apellido = "unApellidoDos", Email = "unCorreoDos@gmail.com",
            Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!"
        };
        _controladorUsuario.AgregarUsuario(nuevoUsuario);
        _controladorUsuario.AgregarUsuario(nuevoUsuario);
    }

    [TestMethod]
    public void ObtenerTodosLosUsuariosUsuariosEnRepositorioDevuelveUsuariosTest()
    {
        Usuario nuevoUsuario = new Usuario
        {
            Nombre = "OtroNombre", Apellido = "unApellidoDos", Email = "unCorreoDos@gmail.com",
            Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!"
        };
        _controladorUsuario.AgregarUsuario(nuevoUsuario);
        List<Usuario> usuarios = _controladorUsuario.ObtenerTodosLosUsuarios();
        Assert.IsTrue(usuarios.Count > 0);
    }

    [TestMethod]
    public void VerificarSiExisteUnUsuarioTest()
    {
        Usuario nuevoUsuario = new Usuario
        {
            Nombre = "OtroNombre", Apellido = "unApellidoDos", Email = "unCorreoDos@gmail.com",
            Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!"
        };
        _controladorUsuario.AgregarUsuario(nuevoUsuario);
        
        Assert.IsTrue((bool)_controladorUsuario.ExisteUsuario(nuevoUsuario));
    }
}
    