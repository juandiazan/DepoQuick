using Dominio;

namespace DominioTest;

[TestClass]
public class DepositoTest
{
    [TestMethod]
    public void CrearDepositoSinPromocionOk() {
        Deposito unDeposito = new Deposito()
        {
            Area = AreasDeDeposito.A,
            Tamanio = TamaniosDeDeposito.Pequeño,
            Climatizacion = true
        };
    }

    [TestMethod] 
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConAreaInvalida() {
        Deposito unDeposito = new Deposito()
        {
            Area = (AreasDeDeposito) 10,
            Tamanio = TamaniosDeDeposito.Grande,
            Climatizacion = false
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConTamanioInvalido() {
        Deposito unDeposito = new Deposito()
        {
            Area = AreasDeDeposito.B,
            Tamanio = (TamaniosDeDeposito)15,
            Climatizacion = true
        };
    }
    
    [TestMethod]
    public void CrearDepositoConUnaPromocionOk() {
        Promocion unaPromocion = new Promocion();
        Deposito unDeposito = new Deposito();
        
        unDeposito.AgregarPromocion(unaPromocion);
        
        Assert.AreEqual(1, unDeposito.ListaPromociones.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConPromocionesQueSuperenElCienPorcientoDeDescuento() {
        Promocion unaPromocion = new Promocion()
        {
            PorcentajeDescuento = 55
        };
        Promocion otraPromocion = new Promocion()
        {
            PorcentajeDescuento = 60
        };

        Deposito unDeposito = new Deposito();
        
        unDeposito.AgregarPromocion(unaPromocion);
        unDeposito.AgregarPromocion(otraPromocion);
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoAgregandoDosVecesLaMismaPromocion() {
        Promocion unaPromocion = new Promocion()
        {
            PorcentajeDescuento = 30
        };

        Deposito unDeposito = new Deposito();
        
        unDeposito.AgregarPromocion(unaPromocion);
        unDeposito.AgregarPromocion(unaPromocion);
    }

    [TestMethod]
    public void CrearDepositoConNombreOkTest() {
        Deposito unDeposito = new Deposito()
        {
            Nombre = "Deposito"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConNombreVacioTest() {
        Deposito unDeposito = new Deposito()
        {
            Nombre = ""
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConNombreConEspaciosTest() {
        Deposito unDeposito = new Deposito()
        {
            Nombre = "Depo    sito"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConNombreConNumerosTest() {
        Deposito unDeposito = new Deposito()
        {
            Nombre = "Deposito1"
        };
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConNombreConSimbolosTest() {
        Deposito unDeposito = new Deposito()
        {
            Nombre = "Deposito!@#$%.,"
        };
    }

    [TestMethod]
    public void CrearDepositoConDisponibilidadVaciaOkTest() {
        List<RangoDeFechas> disponibilidadesDeDeposito = new List<RangoDeFechas>();

        Deposito unDeposito = new Deposito();
    }

    [TestMethod]
    public void CrearDepositoConDisponibilidadDeUnaSemanaOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);

        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(unaDisponibilidad));
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConMismoRangoDosVecesTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        otraDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioException))]
    public void CrearDepositoConDisponibilidadErronea() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = DateTime.Now;
        unaDisponibilidad.FechaFin = DateTime.Today.AddDays(-5);

        Deposito unDeposito = new Deposito();
        
        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
    }

    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void CrearDepositoConDisponibilidadIncluidaEnOtraExistenteTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 1, 2);
        otraDisponibilidad.FechaFin = otraDisponibilidad.FechaInicio.AddDays(3);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());
    }

    [TestMethod]
    public void CrearDepositoConDosDisponibilidadesOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 2, 1);
        otraDisponibilidad.FechaFin = otraDisponibilidad.FechaInicio.AddDays(7);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());
    }

    [TestMethod]
    public void CrearDepositoConDisponibilidadParcialmenteIncluidaEnOtraYQueSeSumeOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 7);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 1, 5);
        otraDisponibilidad.FechaFin = new DateTime(2024, 1, 15);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());

        RangoDeFechas rangoEsperado = new RangoDeFechas();
        rangoEsperado.FechaInicio = unaDisponibilidad.FechaInicio;
        rangoEsperado.FechaFin = otraDisponibilidad.FechaFin;
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(rangoEsperado));
    }

    [TestMethod]
    public void CrearDepositoConDisponibilidadFechaFinParcialmenteIncluidaEnOtraYQueSeSumeOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 10);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 15);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 1, 5);
        otraDisponibilidad.FechaFin = new DateTime(2024, 1, 12);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());

        RangoDeFechas rangoEsperado = new RangoDeFechas();
        rangoEsperado.FechaInicio = otraDisponibilidad.FechaInicio;
        rangoEsperado.FechaFin = unaDisponibilidad.FechaFin;
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(rangoEsperado));
    }

    [TestMethod]
    public void AgregarDisponibilidadMasAmpliaQueExistenteOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 10);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 15);

        RangoDeFechas disponibilidadAntes = new RangoDeFechas();
        disponibilidadAntes.FechaInicio = new DateTime(2024, 1, 1);
        disponibilidadAntes.FechaFin = new DateTime(2024, 1, 12);
        
        RangoDeFechas disponibilidadDespues = new RangoDeFechas();
        disponibilidadDespues.FechaInicio = new DateTime(2024, 1, 14);
        disponibilidadDespues.FechaFin = new DateTime(2024, 1, 20);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(disponibilidadAntes, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(disponibilidadDespues, new List<Reserva>());

        RangoDeFechas rangoEsperado = new RangoDeFechas();
        rangoEsperado.FechaInicio = new DateTime(2024, 1, 1);
        rangoEsperado.FechaFin = new DateTime(2024, 1, 20);
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(rangoEsperado));
        Assert.AreEqual(1, unDeposito.CantidadFechasDisponibilidad());
    }

    [TestMethod]
    public void AgregarDisponibilidadIncluidaEnDosDistintasOkTest() {
        RangoDeFechas primeraDisponibilidad = new RangoDeFechas();
        primeraDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        primeraDisponibilidad.FechaFin = new DateTime(2024, 1, 10);

        RangoDeFechas segundaDisponibilidad = new RangoDeFechas();
        segundaDisponibilidad.FechaInicio = new DateTime(2024, 1, 20);
        segundaDisponibilidad.FechaFin = new DateTime(2024, 1, 30);
        
        RangoDeFechas terceraDisponibilidad = new RangoDeFechas();
        terceraDisponibilidad.FechaInicio = new DateTime(2024, 1, 9);
        terceraDisponibilidad.FechaFin = new DateTime(2024, 1, 21);

        Deposito unDeposito = new Deposito();
        
        unDeposito.AgregarFechaDeDisponibilidad(primeraDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(segundaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(terceraDisponibilidad, new List<Reserva>());

        RangoDeFechas rangoEsperado = new RangoDeFechas();
        rangoEsperado.FechaInicio = new DateTime(2024, 1, 1);
        rangoEsperado.FechaFin = new DateTime(2024, 1, 30);
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(rangoEsperado));
        Assert.AreEqual(1, unDeposito.CantidadFechasDisponibilidad());
    }

    [TestMethod]
    public void ObtenerDisponibilidadesOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = unaDisponibilidad.FechaInicio.AddDays(7);

        RangoDeFechas otraDisponibilidad = new RangoDeFechas();
        otraDisponibilidad.FechaInicio = new DateTime(2024, 2, 1);
        otraDisponibilidad.FechaFin = otraDisponibilidad.FechaInicio.AddDays(7);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());
        unDeposito.AgregarFechaDeDisponibilidad(otraDisponibilidad, new List<Reserva>());

        List<RangoDeFechas> disponibilidades = unDeposito.ObtenerDisponibilidades();
        
        Assert.AreEqual(2, disponibilidades.Count);
        Assert.IsTrue(disponibilidades.Contains(unaDisponibilidad));
    }

    [TestMethod]
    public void ReservarDepositoYLuegoAgregarleDisponibilidadQueNoEntreEnReservaOkTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 10);
        
        Deposito unDeposito = new Deposito();

        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, new List<Reserva>());

        RangoDeFechas unaReserva = new RangoDeFechas();
        unaReserva.FechaInicio = new DateTime(2024, 1, 3);
        unaReserva.FechaFin = new DateTime(2024, 1, 5);
        
        unDeposito.ReservarFecha(unaReserva);
        
        RangoDeFechas nuevaFecha = new RangoDeFechas();
        nuevaFecha.FechaInicio = new DateTime(2024, 1, 15);
        nuevaFecha.FechaFin = new DateTime(2024, 1, 17);
        
        unDeposito.AgregarFechaDeDisponibilidad(nuevaFecha, new List<Reserva>());
        
        Assert.IsTrue(unDeposito.EstaDisponibleEnRango(nuevaFecha));
        Assert.AreEqual(3, unDeposito.CantidadFechasDisponibilidad());
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void ReservarDepositoYLuegoAgregarleDisponibilidadConflictivaConReservaTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 30);

        List<Reserva> listaReservas = new List<Reserva>();
        
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, listaReservas);

        RangoDeFechas fechaReserva = new RangoDeFechas();
        fechaReserva.FechaInicio = new DateTime(2024, 1, 10);
        fechaReserva.FechaFin = new DateTime(2024, 1, 20);

        unDeposito.ReservarFecha(fechaReserva);
        listaReservas.Add(new Reserva() {RangoDeFechas = fechaReserva, Deposito = unDeposito});
        
        RangoDeFechas nuevaFecha = new RangoDeFechas();
        nuevaFecha.FechaInicio = new DateTime(2024, 1, 15);
        nuevaFecha.FechaFin = new DateTime(2024, 1, 17);
        
        unDeposito.AgregarFechaDeDisponibilidad(nuevaFecha, listaReservas);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void ReservarDepositoYLuegoAgregarleDisponibilidadConflictivaConReservaFechaInicioTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 30);

        List<Reserva> listaReservas = new List<Reserva>();
        
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, listaReservas);

        RangoDeFechas fechaReserva = new RangoDeFechas();
        fechaReserva.FechaInicio = new DateTime(2024, 1, 10);
        fechaReserva.FechaFin = new DateTime(2024, 1, 20);

        unDeposito.ReservarFecha(fechaReserva);
        listaReservas.Add(new Reserva() {RangoDeFechas = fechaReserva, Deposito = unDeposito});
        
        RangoDeFechas nuevaFecha = new RangoDeFechas();
        nuevaFecha.FechaInicio = new DateTime(2024, 1, 15);
        nuevaFecha.FechaFin = new DateTime(2024, 1, 25);
        
        unDeposito.AgregarFechaDeDisponibilidad(nuevaFecha, listaReservas);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DominioDepositoException))]
    public void ReservarDepositoYLuegoAgregarleDisponibilidadConflictivaConReservaFechaFinTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 1);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 30);

        List<Reserva> listaReservas = new List<Reserva>();
        
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, listaReservas);

        RangoDeFechas fechaReserva = new RangoDeFechas();
        fechaReserva.FechaInicio = new DateTime(2024, 1, 10);
        fechaReserva.FechaFin = new DateTime(2024, 1, 20);

        unDeposito.ReservarFecha(fechaReserva);
        listaReservas.Add(new Reserva() {RangoDeFechas = fechaReserva, Deposito = unDeposito});
        
        RangoDeFechas nuevaFecha = new RangoDeFechas();
        nuevaFecha.FechaInicio = new DateTime(2024, 1, 5);
        nuevaFecha.FechaFin = new DateTime(2024, 1, 15);
        
        unDeposito.AgregarFechaDeDisponibilidad(nuevaFecha, listaReservas);
    }
    
    [TestMethod]
    public void ReservarDepositoYLuegoAgregarleDisponibilidadMayorATiempoDeReservaTest() {
        RangoDeFechas unaDisponibilidad = new RangoDeFechas();
        unaDisponibilidad.FechaInicio = new DateTime(2024, 1, 10);
        unaDisponibilidad.FechaFin = new DateTime(2024, 1, 20);

        List<Reserva> listaReservas = new List<Reserva>();
        
        Deposito unDeposito = new Deposito();
        unDeposito.AgregarFechaDeDisponibilidad(unaDisponibilidad, listaReservas);

        RangoDeFechas fechaReserva = new RangoDeFechas();
        fechaReserva.FechaInicio = new DateTime(2024, 1, 14);
        fechaReserva.FechaFin = new DateTime(2024, 1, 16);

        unDeposito.ReservarFecha(fechaReserva);
        listaReservas.Add(new Reserva() {RangoDeFechas = fechaReserva, Deposito = unDeposito, EstadoAprobacionAdmin = true, EstadoAprobacionCliente = true});
        
        RangoDeFechas nuevaFecha = new RangoDeFechas();
        nuevaFecha.FechaInicio = new DateTime(2024, 1, 5);
        nuevaFecha.FechaFin = new DateTime(2024, 1, 25);
        
        unDeposito.AgregarFechaDeDisponibilidad(nuevaFecha, listaReservas);
        
        Assert.AreEqual(2, unDeposito.CantidadFechasDisponibilidad());
    }
}
