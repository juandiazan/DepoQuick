namespace Dominio;

public class Reserva
{
    public int Id { get; set; }
    public RangoDeFechas RangoDeFechas { get; set; }
    public Usuario Usuario { get; set; }
    public Deposito Deposito { get; set; }    
    public double Costo { get; set; }
    public bool EstadoAprobacionCliente { get; set; }
    public bool EstadoAprobacionAdmin { get; set; }
    public string MotivoRechazo { get; set; }
    public Pago Pago { get; set; }
    
    public override string ToString() {
        return $"Usuario: {Usuario.Nombre} {Usuario.Apellido}, Rango de fechas: {RangoDeFechas.FechaInicio.ToString("dd/MM/yyyy")} - {RangoDeFechas.FechaFin.ToString("dd/MM/yyyy")}";
    }
}