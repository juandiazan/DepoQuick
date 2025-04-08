using Dominio;

namespace LogicaNegocio;

public static class ExportadorReporteFactory
{
    private static readonly Dictionary<FormatoExportacion, IExportadorReporte<Reserva>> _exportadores;
    
    static ExportadorReporteFactory()
    {
        _exportadores = new Dictionary<FormatoExportacion, IExportadorReporte<Reserva>>
        {
            { FormatoExportacion.csv, new ExportadorCsv() },
            { FormatoExportacion.txt, new ExportadorTxt() }
        };
    }
    
    public static IExportadorReporte<Reserva> CrearExportador(FormatoExportacion formato)
    {
        return _exportadores[formato];
    }
}