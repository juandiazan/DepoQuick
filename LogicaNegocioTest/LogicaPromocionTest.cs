using Dominio;
using LogicaNegocio;
using LogicaNegocioTest.ContextoBD;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaPromocionTest
{
    private ContextoSql _contexto;
    
    private LogicaSesion _logicaSesion;
    private LogicaUsuario _logicaUsuario;
    private LogicaPromocion _logicaPromocion;
    private LogicaDeposito _logicaDeposito;

    private Usuario _adminParaInicioSesion;
    
    [TestInitialize]
    public void SetUp() {
        _contexto = ContextoSqlFactory.CrearContextoEnMemoria();
        
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(new RepositorioUsuarioBaseDeDatos(_contexto), _logicaSesion);
        _logicaPromocion = new LogicaPromocion(new RepositorioPromocionBaseDeDatos(_contexto), _logicaSesion);
        _logicaDeposito = new LogicaDeposito(new RepositorioDepositoBaseDeDatos(_contexto), _logicaSesion);
        
        _adminParaInicioSesion = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };

        _logicaUsuario.Add(_adminParaInicioSesion);
    }

    [TestCleanup]
    public void CleanUp() {
        _contexto.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AgregarUnaPromocionOkTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        Assert.AreEqual(1, promocionRetorno.Id);
        Assert.AreEqual(promocion.Etiqueta, promocionRetorno.Etiqueta);
        Assert.AreEqual(promocion.PorcentajeDescuento, promocionRetorno.PorcentajeDescuento);
        Assert.AreEqual(promocion.rangoFechas.FechaInicio, promocionRetorno.rangoFechas.FechaInicio);
        Assert.AreEqual(promocion.rangoFechas.FechaFin, promocionRetorno.rangoFechas.FechaFin);
    }
    
    [TestMethod]
    public void AgregarDosPromocionesOkTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion1 = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
        
        RangoDeFechas rango2 = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 11, 1);
        rango.FechaFin = new DateTime(2024, 11, 30);
        
        Promocion promocion2 = new Promocion()
        {
            Etiqueta = "Promo2",
            PorcentajeDescuento = 20,
            rangoFechas = rango2
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        Promocion promocionRetorno1 = _logicaPromocion.Add(promocion1);
        Promocion promocionRetorno2 = _logicaPromocion.Add(promocion2);
        
        Assert.AreEqual(1, promocionRetorno1.Id);
        Assert.AreEqual(promocion1.Etiqueta, promocionRetorno1.Etiqueta);
        Assert.AreEqual(promocion1.PorcentajeDescuento, promocionRetorno1.PorcentajeDescuento);
        Assert.AreEqual(promocion1.rangoFechas.FechaInicio, promocionRetorno1.rangoFechas.FechaInicio);
        Assert.AreEqual(promocion1.rangoFechas.FechaFin, promocionRetorno1.rangoFechas.FechaFin);
        
        Assert.AreEqual(2, promocionRetorno2.Id);
        Assert.AreEqual(promocion2.Etiqueta, promocionRetorno2.Etiqueta);
        Assert.AreEqual(promocion2.PorcentajeDescuento, promocionRetorno2.PorcentajeDescuento);
        Assert.AreEqual(promocion2.rangoFechas.FechaInicio, promocionRetorno2.rangoFechas.FechaInicio);
        Assert.AreEqual(promocion2.rangoFechas.FechaFin, promocionRetorno2.rangoFechas.FechaFin);
    }
    
    [TestMethod]
    public void EliminarUnaPromocionOkTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), promocionRetorno);
        
        Assert.AreEqual(0, _logicaPromocion.GetAll().Count);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionInexistenteTest ()
    {
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), new Promocion());
    }
    
    [TestMethod]
    public void ModificarPromocionOkTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        promocionRetorno.PorcentajeDescuento = 20;
        
        Promocion promocionModificada = _logicaPromocion.Update(promocionRetorno);
        
        Assert.AreEqual(1, promocionModificada.Id);
        Assert.AreEqual(promocionRetorno.Etiqueta, promocionModificada.Etiqueta);
        Assert.AreEqual(promocionRetorno.PorcentajeDescuento, promocionModificada.PorcentajeDescuento);
        Assert.AreEqual(promocionRetorno.rangoFechas.FechaInicio, promocionModificada.rangoFechas.FechaInicio);
        Assert.AreEqual(promocionRetorno.rangoFechas.FechaFin, promocionModificada.rangoFechas.FechaFin);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void ModificarPromocionInexistenteTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
        
        _logicaPromocion.Update(promocion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void ModificarPromocionConPorcentajeDescuentoMenorA5Test ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        promocionRetorno.PorcentajeDescuento = 4;
        
        _logicaPromocion.Update(promocionRetorno);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioPromocionException))]
    public void ModificarPromocionConPorcentajeDescuentoMayorA75 ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        promocionRetorno.PorcentajeDescuento = 88;
        
        _logicaPromocion.Update(promocionRetorno);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioException))]
    public void ModificarPromocionConFechaInicioPosteriorAFechaFinTest ()
    {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 31);
        rango.FechaFin = new DateTime(2024, 10, 1);
        
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };
        
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        
        promocionRetorno.rangoFechas.FechaInicio = new DateTime(2024, 11, 1);
        
        _logicaPromocion.Update(promocionRetorno);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void AgregarPromocionCuandoNoHayUsuarioEnSesion() {
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 31);
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
            rangoFechas = rango
        };

        _logicaPromocion.Add(unaPromocion);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void AgregarPromocionCuandoHayClienteEnSesion() {
        
        Promocion unaPromocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };

        Usuario unCliente = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(unCliente);
        _logicaSesion.IniciarSesion(unCliente, unCliente.Contrasena, _logicaUsuario.GetAll());
        
        _logicaPromocion.Add(unaPromocion);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionCuandoNoHayUsuarioEnSesion() {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        _logicaSesion.CerrarSesion();
        
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), promocionRetorno);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionCuandoHayClienteEnSesion() {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };
        
        Usuario unCliente = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(unCliente);
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        _logicaSesion.CerrarSesion();
        
        _logicaSesion.IniciarSesion(unCliente, unCliente.Contrasena, _logicaUsuario.GetAll());
        
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), promocionRetorno);   
    }

    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void ModificarPromocionCuandoNoHayUsuarioEnSesion() {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };

        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        _logicaSesion.CerrarSesion();
        
        promocionRetorno.PorcentajeDescuento = 20;
        
        _logicaPromocion.Update(promocionRetorno);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void ModificarPromocionCuandoHayClienteEnSesion() {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };
        
        Usuario unCliente = new Usuario()
        {
            Email = "unCorreo@nuevoCorreo",
            Nombre = "unNombre",
            Apellido = "unApellido",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(unCliente);
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        Promocion promocionRetorno = _logicaPromocion.Add(promocion);
        _logicaSesion.CerrarSesion();
        
        promocionRetorno.PorcentajeDescuento = 20;
        
        _logicaSesion.IniciarSesion(unCliente, unCliente.Contrasena, _logicaUsuario.GetAll());
        _logicaPromocion.Update(promocionRetorno);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionAplicadaAUnDeposito() {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };

        Deposito unDeposito = new Deposito()
        {
            Nombre = "unNombre"
        };
        unDeposito.AgregarPromocion(promocion);
        
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaPromocion.Add(promocion);
        
        _logicaDeposito.Add(unDeposito);
        
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), promocion);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void EliminarPromocionClienteEnSesion()
    {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaPromocion.Delete(_logicaDeposito.GetAll(), promocion);
    }
    
    [TestMethod]
    public void FindPromocionExistente()
    {
        Promocion promocion = new Promocion()
        {
            Etiqueta = "Promo1",
            PorcentajeDescuento = 10,
        };
        _logicaSesion.IniciarSesion(_adminParaInicioSesion, _adminParaInicioSesion.Contrasena, _logicaUsuario.GetAll());
        _logicaPromocion.Add(promocion);

        Promocion promoEncontrada = _logicaPromocion.Find(p => p.Id == promocion.Id);

        Assert.IsNotNull(promoEncontrada);
        Assert.AreEqual(promocion.Id, promoEncontrada.Id);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicaPromocionException))]
    public void FindPromocionNoExistenteTiraExcepcion()
    {
        _logicaPromocion.Find(p => p.Id == -1);
    }
}