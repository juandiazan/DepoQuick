using Dominio;
using LogicaNegocio;
using Repositorio;

namespace LogicaNegocioTest;

[TestClass]
public class LogicaPrecioDepositoTest
{
    private LogicaUsuario _logicaUsuario;
    private LogicaDeposito _logicaDeposito;
    private LogicaSesion _logicaSesion;
    private LogicaCalculoPrecio _logicaCalculoPrecio;
    
    private Usuario _adminParaInicioSesion;
    private Usuario _clienteParaIniciarSesion;
    private List<Usuario> _listaUsuarios;
    
    [TestInitialize]
    public void SetUp() {
        _logicaSesion = new LogicaSesion();
        _logicaUsuario = new LogicaUsuario(new RepositorioUsuario(), _logicaSesion);
        _logicaDeposito = new LogicaDeposito(new RepositorioDeposito(), _logicaSesion);
        _logicaCalculoPrecio = new LogicaCalculoPrecio();
        
        _adminParaInicioSesion = new Usuario()
        {
            Email = "unCorreo@unCorreo",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Administrador
        };

        _clienteParaIniciarSesion = new Usuario()
        {
            Email = "unCorreo1@unCorreo",
            Contrasena = "UnaPass123!",
            VerificacionContrasena = "UnaPass123!",
            RolDeUsuario = Rol.Cliente
        };
        
        _logicaUsuario.Add(_adminParaInicioSesion);
        _logicaUsuario.Add(_clienteParaIniciarSesion);

        _listaUsuarios = _logicaUsuario.GetAll();
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioException))]
    public void CalcularPrecioBaseDepositoPequenoPorCeroDiasSinClimNiOfertaTest() {
        Deposito depositoPequeno = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Pequeño
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 9, 1);
        
        _logicaCalculoPrecio.CalcularPrecioBase(depositoPequeno, rango);
    }
    
    [TestMethod]
    public void CalcularPrecioBaseDepositoPequenoPorUnDiaSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoPequeno = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Pequeño
        };
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 2);

        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoPequeno, rango);

        Assert.AreEqual(50, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoMedianoPorUnDiaSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoMediano = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Mediano
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 2);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoMediano, rango);

        Assert.AreEqual(75, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoGrandePorUnDiaSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoGrande = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Grande
        };
        
        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 2);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoGrande, rango);

        Assert.AreEqual(100, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoGrandePorSeisDiasSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoGrande = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Grande
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 7);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoGrande, rango);

        Assert.AreEqual(600, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoMedianoPorDiezDiasSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoMediano = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Mediano
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 11);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoMediano, rango);

        Assert.AreEqual(712.5, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoPequenoPorQuinceDiasSinClimNiOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoPequeno = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Pequeño
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 16);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoPequeno, rango);

        Assert.AreEqual(675, precioBase);
    }

    [TestMethod]
    public void CalcularPrecioBaseDepositoGrandePorSieteDiasConClimSinOfertaTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        Deposito depositoGrande = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Grande,
            Climatizacion = true
        };

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 8);
        
        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoGrande, rango);

        Assert.AreEqual(798, precioBase);
    }
    
    [TestMethod]
    public void CalcularPrecioBaseDepositoPequenoPorDiezDiasConOfertaDelVeintePorcientoTest()
    {
        _logicaSesion.IniciarSesion(_clienteParaIniciarSesion, _clienteParaIniciarSesion.Contrasena, _listaUsuarios);

        RangoDeFechas rango = new RangoDeFechas();
        rango.FechaInicio = new DateTime(2024, 10, 1);
        rango.FechaFin = new DateTime(2024, 10, 11);
        
        Deposito depositoPequeno = new Deposito()
        {
            Tamanio = TamaniosDeDeposito.Pequeño,
        };
        
        RangoDeFechas rangoPromo = new RangoDeFechas();
        rangoPromo.FechaInicio = new DateTime(2024, 10, 1);
        rangoPromo.FechaFin = new DateTime(2024, 10, 11);
        
        Promocion promocion = new Promocion()
        {
            PorcentajeDescuento = 20,
            rangoFechas = rangoPromo
        };
        
        depositoPequeno.AgregarPromocion(promocion);

        double precioBase = _logicaCalculoPrecio.CalcularPrecioBase(depositoPequeno, rango);

        Assert.AreEqual(380, precioBase);
    }
    
}