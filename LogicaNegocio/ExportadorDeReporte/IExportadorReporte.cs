namespace LogicaNegocio;

public interface IExportadorReporte<T>
{
    byte[] Exportar(List<T> elementos);
}