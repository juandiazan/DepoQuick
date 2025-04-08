namespace Dominio;

public class RangoDeFechas
{
    private DateTime _fechaFin;
 
    
    public int Id { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { 
        get => _fechaFin;
        set
        {
            if (FechaInicioEstaDespuesDeFechaFin(value))
            {
                throw new DominioException("La fecha de fin no puede ser anterior a la fecha de inicio");
            } else if (FechaInicioIgualAFechaFin(value)) 
            {
                throw new DominioException("La fecha de fin no puede ser igual a la fecha de inicio");
            }
            _fechaFin = value;
        } 
    }
    public virtual List<Deposito> listaDepositos { get; set; }
    
    
    private bool FechaInicioEstaDespuesDeFechaFin(DateTime fechaFin) {
        return fechaFin < FechaInicio;
    }
    private bool FechaInicioIgualAFechaFin(DateTime fechaFin) {
        return fechaFin == FechaInicio;
    }
}