namespace Dominio;

public enum AreasDeDeposito {
    A,
    B,
    C,
    D,
    E
}
public enum TamaniosDeDeposito {
    Pequeño,
    Mediano,
    Grande
}

public class Deposito
{
    private const int PorcentajeMaximoDescuento = 100;

    private string _nombre;
    private AreasDeDeposito _area;
    private TamaniosDeDeposito _tamanio;

    public Deposito() {
        ListaDisponibilidad = new List<RangoDeFechas>();
    }
    
    public int Id { get; set; }
    public string Nombre
    {
        get => _nombre;
        set
        {
            if (EsNuloOVacio(value)) {
                throw new DominioDepositoException("El nombre del depósito no puede ser vacío");
            }
            if (TieneMasDeUnEspacioSeguido(value)) {
                throw new DominioDepositoException("El nombre del depósito no puede tener más de un espacio seguido");
            }
            if (TieneCaracteresQueNoSonLetras(value)) {
                throw new DominioDepositoException("El nombre del depósito no puede tener números o símbolos");
            }
            
            _nombre = value;
        }
    }
    public AreasDeDeposito Area
    {
        get => _area;
        set
        {
            if (!EsValidaArea(value)) {
                throw new DominioDepositoException("El área ingresada es inválida");
            }
            _area = value;
        }
    }
    public TamaniosDeDeposito Tamanio
    {
        get => _tamanio;
        set
        {
            if (!EsValidoTamanio(value)) {
                throw new DominioDepositoException("El tamaño ingresado es inválido");
            }
            _tamanio = value;
        }
    }
    public bool Climatizacion { get; set; }
    public List<Promocion> ListaPromociones = new List<Promocion>();
    public List<RangoDeFechas> ListaDisponibilidad { get; set; }
    
    
    public void AgregarPromocion(Promocion unaPromocion) {
        int descuentoTotalMasNuevaPromocion = CalcularPorcentajeActual() + unaPromocion.PorcentajeDescuento;
        
        if (SuperaPorcentajeMaximoDeDescuento(descuentoTotalMasNuevaPromocion)) {
            throw new DominioDepositoException("La promoción agregada supera el porcentaje máximo de descuento");
        } else if (YaTieneLaPromocion(unaPromocion)) {
            throw new DominioDepositoException("A este depósito ya se le aplicó esta promoción");
        }
        ListaPromociones.Add(unaPromocion);
    }
    public void AgregarFechaDeDisponibilidad(RangoDeFechas unaDisponibilidad, List<Reserva> listaReservas) {
        if (EstaDisponibleEnRango(unaDisponibilidad)) {
            throw new DominioDepositoException("El depósito ya está disponible en ese rango de fechas");
        }
        if (ExisteReservaEnNuevaFecha(unaDisponibilidad, listaReservas)) {
            throw new DominioDepositoException("Hay una reserva realizada para la disponibilidad ingresada");
        }
        
        if (NuevaFechaTieneFechaInicioIncluidaEnDisponibilidadExistente(unaDisponibilidad)) {
            ModificarFechaInicio(unaDisponibilidad);
        } else if (NuevaFechaTieneFechaFinIncluidaEnDisponibilidadExistente(unaDisponibilidad)) {
            ModificarFechaFin(unaDisponibilidad);
        }
        
        ListaDisponibilidad.Add(unaDisponibilidad);
        CombinarRangos(listaReservas);
    }
    public void ReservarFecha(RangoDeFechas unaReserva) {
        List<RangoDeFechas> nuevasDisponibilidades = new List<RangoDeFechas>();
        
        foreach (var disponibilidad in ListaDisponibilidad) {
            if (ReservaFueraDeRangoDeDisponibilidad(unaReserva, disponibilidad)) {
                nuevasDisponibilidades.Add(disponibilidad);
            } else {
                QuitarDisponibilidadPorReserva(nuevasDisponibilidades, unaReserva, disponibilidad);
            }
        }
        ListaDisponibilidad = nuevasDisponibilidades;
    }
    public bool EstaDisponibleEnRango(RangoDeFechas unRango) {
        return ListaDisponibilidad.Contains(unRango) || NuevaFechaIncluidaEnDisponibilidadExistente(unRango);
    }
    public int CantidadFechasDisponibilidad() {
        return ListaDisponibilidad.Count;
    }
    public List<RangoDeFechas> ObtenerDisponibilidades() {
        return ListaDisponibilidad;
    }
    public override string ToString() {
        return $"ID: {Id} - Área: {Area} - Tamaño: {Tamanio} - {(Climatizacion ? "Con climatización" : "Sin climatización")} - {(ListaPromociones.Count > 0 ? "Con promoción" : "Sin promoción")}";
    }
    
    
    private bool TieneMasDeUnEspacioSeguido(string unNombre) {
        return unNombre.Contains("  ");
    }
    private bool TieneCaracteresQueNoSonLetras(string unNombre) {
        return unNombre.Any(unCaracter => !char.IsLetter(unCaracter));
    }
    private bool EsNuloOVacio(string unNombre) {
        return string.IsNullOrEmpty(unNombre);
    }
    private bool EsValidaArea(AreasDeDeposito unArea) {
        return Enum.IsDefined(typeof(AreasDeDeposito), unArea);
    }
    private bool EsValidoTamanio(TamaniosDeDeposito unTamanio) {
        return Enum.IsDefined(typeof(TamaniosDeDeposito), unTamanio);
    }
    private int CalcularPorcentajeActual() {
        int porcentajeDeDescuentoTotal = 0;
        
        foreach (Promocion promo in ListaPromociones) {
            porcentajeDeDescuentoTotal += promo.PorcentajeDescuento;
        }
        
        return porcentajeDeDescuentoTotal;
    }
    private bool SuperaPorcentajeMaximoDeDescuento(int unPorcentaje) {
        return unPorcentaje > PorcentajeMaximoDescuento;
    }
    private bool YaTieneLaPromocion(Promocion unaPromocion) {
        return ListaPromociones.Contains(unaPromocion);
    }
    private bool ExisteReservaEnNuevaFecha(RangoDeFechas unaDisponibilidad, List<Reserva> listaReservas) {
        return listaReservas.Any(reserva => 
            (reserva.Deposito.Id == Id && FechaIncluidaEnOtra(unaDisponibilidad, reserva.RangoDeFechas)));
    }
    private bool FechaIncluidaEnOtra(RangoDeFechas primeraFecha, RangoDeFechas segundaFecha) {
        return FechaIncluidaTotalmenteEnOtra(primeraFecha, segundaFecha) ||
               FechaIncluidaParcialmenteEnOtra(primeraFecha, segundaFecha);
    }
    private bool FechaIncluidaTotalmenteEnOtra(RangoDeFechas primeraFecha, RangoDeFechas segundaFecha) {
        return primeraFecha.FechaInicio.Date >= segundaFecha.FechaInicio.Date
               && primeraFecha.FechaFin.Date <= segundaFecha.FechaFin.Date;
    }
    private bool FechaIncluidaParcialmenteEnOtra(RangoDeFechas primeraFecha, RangoDeFechas segundaFecha) {
        return FechaInicioIncluidaEnOtraFecha(primeraFecha, segundaFecha) || 
               FechaFinIncluidaEnOtraFecha(primeraFecha, segundaFecha);
    }
    private bool FechaInicioIncluidaEnOtraFecha(RangoDeFechas primeraFecha, RangoDeFechas segundaFecha) {
        return primeraFecha.FechaFin.Date > segundaFecha.FechaFin.Date &&
               primeraFecha.FechaInicio.Date >= segundaFecha.FechaInicio.Date &&
               primeraFecha.FechaInicio.Date <= segundaFecha.FechaFin.Date;
    }
    private bool FechaFinIncluidaEnOtraFecha(RangoDeFechas primeraFecha, RangoDeFechas segundaFecha) {
        return primeraFecha.FechaInicio.Date < segundaFecha.FechaInicio.Date &&
               primeraFecha.FechaFin.Date >= segundaFecha.FechaInicio.Date &&
               primeraFecha.FechaFin.Date <= segundaFecha.FechaFin.Date;
    }
    private bool NuevaFechaTieneFechaInicioIncluidaEnDisponibilidadExistente(RangoDeFechas unaDisponibilidad) {
        return ListaDisponibilidad.Any(rango =>
            FechaInicioIncluidaEnOtraFecha(unaDisponibilidad, rango));
    }
    private void ModificarFechaInicio(RangoDeFechas unaDisponibilidad) {
        RangoDeFechas rango = ListaDisponibilidad.Find(rango => rango.FechaInicio <= unaDisponibilidad.FechaInicio && rango.FechaFin < unaDisponibilidad.FechaFin);
        rango.FechaFin = unaDisponibilidad.FechaFin;
    }
    private bool NuevaFechaTieneFechaFinIncluidaEnDisponibilidadExistente(RangoDeFechas unaDisponibilidad) {
        return ListaDisponibilidad.Any(rango =>
            FechaFinIncluidaEnOtraFecha(unaDisponibilidad, rango));
    }
    private void ModificarFechaFin(RangoDeFechas unaDisponibilidad) {
        RangoDeFechas rango = ListaDisponibilidad.Find(rango => rango.FechaInicio > unaDisponibilidad.FechaInicio && rango.FechaFin >= unaDisponibilidad.FechaFin);
        rango.FechaInicio = unaDisponibilidad.FechaInicio;
    }
    private void CombinarRangos(List<Reserva> reservas) {
        ListaDisponibilidad.Sort((a, b) => a.FechaInicio.CompareTo(b.FechaInicio));

        List<RangoDeFechas> nuevasDisponibilidades = new List<RangoDeFechas>();
        RangoDeFechas rangoActual = ListaDisponibilidad[0];
        for (int i = 1; i < ListaDisponibilidad.Count; i++) {
            if (ListaDisponibilidad[i].FechaInicio <= rangoActual.FechaFin.AddDays(1)) {
                rangoActual.FechaFin = new DateTime(Math.Max(rangoActual.FechaFin.Ticks, ListaDisponibilidad[i].FechaFin.Ticks));
            }
            else {
                nuevasDisponibilidades.Add(rangoActual);
                rangoActual = ListaDisponibilidad[i];
            }
        }
        nuevasDisponibilidades.Add(rangoActual);
        
        ListaDisponibilidad = nuevasDisponibilidades;
        
        ActualizarListaDisponibilidades(reservas);
    }
    private void ActualizarListaDisponibilidades(List<Reserva> reservas) {
        foreach (Reserva unaReserva in reservas) {
            if (Id == unaReserva.Deposito.Id) {
                ReservarFecha(unaReserva.RangoDeFechas);
            }
        }
    }
    private bool ReservaFueraDeRangoDeDisponibilidad(RangoDeFechas unaReserva, RangoDeFechas unaDisponibilidad) {
        return unaReserva.FechaFin < unaDisponibilidad.FechaInicio ||
               unaReserva.FechaInicio > unaDisponibilidad.FechaFin;
    }
    private void QuitarDisponibilidadPorReserva(List<RangoDeFechas> listaFechas, RangoDeFechas unaReserva, RangoDeFechas disponibilidad) {
        bool reservaEmpiezaLuegoDeDisponibilidad = unaReserva.FechaInicio.Date > disponibilidad.FechaInicio.Date;
        bool reservaTerminaAntesDeDisponibilidad = unaReserva.FechaFin.Date < disponibilidad.FechaFin.Date;
        
        if (reservaEmpiezaLuegoDeDisponibilidad) {
            DateTime inicioDeRangoDeDisponibilidad = disponibilidad.FechaInicio;
            DateTime diaAnteriorAInicioDeReserva = unaReserva.FechaInicio.AddDays(-1);
            
            if (inicioDeRangoDeDisponibilidad != diaAnteriorAInicioDeReserva) {
                RangoDeFechas disponibilidadAntes = new RangoDeFechas();
                disponibilidadAntes.FechaInicio = inicioDeRangoDeDisponibilidad;
                disponibilidadAntes.FechaFin = diaAnteriorAInicioDeReserva;
                listaFechas.Add(disponibilidadAntes);
            }
        }
        if (reservaTerminaAntesDeDisponibilidad) {
            DateTime diaSiguienteAFinDeReserva = unaReserva.FechaFin.AddDays(1);
            DateTime finDeRangoDeDisponibilidad = disponibilidad.FechaFin;
            
            if (diaSiguienteAFinDeReserva != finDeRangoDeDisponibilidad) {
                RangoDeFechas disponibilidadDespues = new RangoDeFechas();
                disponibilidadDespues.FechaInicio = diaSiguienteAFinDeReserva;
                disponibilidadDespues.FechaFin = finDeRangoDeDisponibilidad;
                listaFechas.Add(disponibilidadDespues);
            }
        }
    } 
    private bool NuevaFechaIncluidaEnDisponibilidadExistente(RangoDeFechas unRango) {
        return ListaDisponibilidad.Any(rango =>
            FechaIncluidaTotalmenteEnOtra(unRango, rango));
    }
}