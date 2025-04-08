using Controladores;
using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace ControladoresTest;

[TestClass]
public class ControladorPromocionTest
{
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    private LogicaPromocion _logicaPromocion;
    private ControladorPromocion _controladorPromocion;
    private ContextoSql _contexto;
    private Usuario _admin;
    private List<Deposito> _depositos;

    [TestInitialize]
    public void Setup()
    {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(new RepositorioUsuarioBaseDeDatos(_contexto), _logicaSesion);
        _logicaPromocion = new LogicaPromocion(new RepositorioPromocionBaseDeDatos(_contexto), _logicaSesion);
        _controladorPromocion = new ControladorPromocion(_logicaPromocion);

        _admin = new Usuario
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@correo.com",
            Contrasena = "@Admin123",
            VerificacionContrasena = "@Admin123",
            RolDeUsuario = Rol.Administrador
        };

        _logicaUsuario.Add(_admin);
        _logicaSesion.IniciarSesion(_admin, _admin.Contrasena, _logicaUsuario.GetAll());

        _depositos = new List<Deposito>
        {
            new Deposito { Id = 1, ListaPromociones = new List<Promocion>() }
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _contexto.Database.EnsureDeleted();
    }

    [TestMethod]
    public void AgregarPromocionNuevaPromocionPromocionAgregadaTest()
    {
        Promocion nuevaPromocion = new Promocion
        {
            Etiqueta = "Promo3",
            PorcentajeDescuento = 30,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        Promocion resultado = _controladorPromocion.AgregarPromocion(nuevaPromocion);

        Assert.AreEqual(nuevaPromocion, resultado);
    }
    
    [TestMethod]
    public void ObtenerTodasLasPromocionesDevuelvePromocionesTest()
    {
        Promocion nuevaPromocion1 = new Promocion
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        Promocion nuevaPromocion2 = new Promocion
        {
            Etiqueta = "Promo2",
            PorcentajeDescuento = 20,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        _controladorPromocion.AgregarPromocion(nuevaPromocion1);
        _controladorPromocion.AgregarPromocion(nuevaPromocion2);

        List<Promocion> promociones = _controladorPromocion.ObtenerTodasLasPromociones();

        Assert.AreEqual(2, promociones.Count);
    }

    [TestMethod]
    public void ActualizarPromocionPromocionExistentePromocionActualizadaTest()
    {
        Promocion promocion = new Promocion
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        _controladorPromocion.AgregarPromocion(promocion);

        promocion.Etiqueta = "Promo Actualizada";
        promocion.PorcentajeDescuento = 15;

        Promocion resultado = _controladorPromocion.ActualizarPromocion(promocion);

        Assert.AreEqual("Promo Actualizada", resultado.Etiqueta);
        Assert.AreEqual(15, resultado.PorcentajeDescuento);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionPromocionEnUsoExcepcionTest()
    {
        Promocion promocion = new Promocion
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        _controladorPromocion.AgregarPromocion(promocion);
        _depositos[0].ListaPromociones.Add(promocion);

        _controladorPromocion.EliminarPromocion(_depositos, promocion);
    }
    
    [TestMethod]
    public void BuscarPromocionPromocionExistenteDevuelvePromocionTest()
    {
        Promocion promocion = new Promocion
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = new RangoDeFechas { FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddMonths(1) }
        };

        _controladorPromocion.AgregarPromocion(promocion);

        Promocion resultado = _controladorPromocion.BuscarPromocion(p => p.Etiqueta == "Promo1");

        Assert.IsNotNull(resultado);
        Assert.AreEqual("Promo1", resultado.Etiqueta);
    }
        
}
