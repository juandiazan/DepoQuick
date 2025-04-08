using System.Text;
using Dominio;

namespace LogicaNegocio;

public class ExportadorTxt : IExportadorReporte<Reserva>
{
    public byte[] Exportar(List<Reserva> elementos)
    {
        StringBuilder txt = new StringBuilder();
        txt.AppendLine("DEPOSITO\tRESERVA\tPAGO");
        
        foreach (Reserva reserva in elementos)
        {
            txt.AppendLine($"{reserva.Deposito}\t{reserva}\t{reserva.Pago.Estado}");
        }
        
        return Encoding.UTF8.GetBytes(txt.ToString());
    }
}