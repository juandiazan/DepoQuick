using Controladores;
using Dominio;
using LogicaNegocio;

namespace ControladoresTest;

[TestClass]
public class ControladorSesionTest
{
    private LogicaSesion _logicaSesion;
    private ControladorSesion _controladorSesion;
    private List<Usuario> _usuarios;

    [TestInitialize]
    public void Setup()
    {
        _logicaSesion = new LogicaSesion();
        _controladorSesion = new ControladorSesion(_logicaSesion);
        _usuarios = new List<Usuario>
        {
            new Usuario { Nombre = "NombreUsuario", Apellido = "unApellido", Email = "unCorreo@gmail.com", Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!", RolDeUsuario = Rol.Administrador },
            new Usuario { Nombre = "OtroNombre", Apellido = "unApellidoDos", Email = "unCorreoDos@gmail.com", Contrasena = "123456Aa#!", VerificacionContrasena = "123456Aa#!", RolDeUsuario = Rol.Cliente }
        };
    }
    
    
    [TestMethod]
    public void IniciarSesionUsuarioExistenteCorrectoTest()
    {
        Usuario usuario = _usuarios [0];
        
        _controladorSesion.IniciarSesion(usuario, "123456Aa#!", _usuarios);
        
        Assert.AreEqual(usuario, _controladorSesion.ObtenerUsuarioEnSesion());
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaUsuarioException))]
    public void IniciarSesionContrasenaIncorrectaExcepcionTest()
    {
        Usuario usuario = _usuarios[0];
        _controladorSesion.IniciarSesion(usuario, "incorrecta", _usuarios);
    }
    
    [TestMethod]
    public void CerrarSesionUsuarioEnSesionCierraSesionTest()
    {
        Usuario usuario = _usuarios[0];
        _controladorSesion.IniciarSesion(usuario, "123456Aa#!", _usuarios);
        _controladorSesion.CerrarSesion();

        Assert.IsNull(_controladorSesion.ObtenerUsuarioEnSesion());
    }
    
    [TestMethod]
    public void HayUnClienteEnSesionClienteEnSesionDevuelveVerdaderoTest()
    {
        Usuario usuario = _usuarios[1];
        _controladorSesion.IniciarSesion(usuario, "123456Aa#!", _usuarios);

        Assert.IsTrue(_controladorSesion.HayUnClienteEnSesion());
    }
    
    [TestMethod]
    public void HayUnaSesionIniciadaSesionIniciadaDevuelveVerdaderoTest()
    {
        Usuario usuario = _usuarios[0];
        _controladorSesion.IniciarSesion(usuario, "123456Aa#!", _usuarios);

        Assert.IsTrue(_controladorSesion.HayUnaSesionIniciada());
    }
    
}