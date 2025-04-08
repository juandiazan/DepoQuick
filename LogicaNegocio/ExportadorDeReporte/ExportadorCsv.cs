using System.Text;
using Dominio;

namespace LogicaNegocio;

public class ExportadorCsv : IExportadorReporte<Reserva>
{
    public byte[] Exportar(List<Reserva> elementos)
    {
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("DEPOSITO,RESERVA,PAGO");
        
        foreach (Reserva reserva in elementos)
        {
            csv.AppendLine($"{reserva.Deposito},{reserva},{reserva.Pago.Estado}");
        }
        
        return Encoding.UTF8.GetBytes(csv.ToString());
    }
}