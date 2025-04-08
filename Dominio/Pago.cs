namespace Dominio;

public class Pago
{
    private double _monto;
    private string _estado;
    
    
    public int Id { get; set; }
    public double Monto
    {
        get => _monto;
        set
        {
            if (EsNegativo(value))
            {
                throw new DominioPagoException("El monto del pago no puede ser negativo");
            }
            _monto = value;
        }
    }
    public string Estado
    {
        get => _estado;
        set
        {
            if (EsNuloOVacio(value))
            {
                throw new DominioPagoException("El estado no puede ser vacío");
            }
            _estado = value;
        }
    }

    
    private bool EsNegativo(double value)
    {
        return value < 0;
    }
    private bool EsNuloOVacio(string value)
    {
        return string.IsNullOrEmpty(value);
    }
}