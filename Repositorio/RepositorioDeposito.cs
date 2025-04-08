using Dominio;

namespace Repositorio;

public class RepositorioDeposito : IRepositorioFindDelete<Deposito>
{
    private List<Deposito> _listaDepositos = new List<Deposito>();
    public Deposito Add(Deposito nuevoDeposito) {
        nuevoDeposito.Id = _listaDepositos.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _listaDepositos.Add(nuevoDeposito);
        return nuevoDeposito;
    }
    public void Delete(Deposito unDeposito) {
        _listaDepositos.RemoveAll(x => x.Id == unDeposito.Id);
    }
    public List<Deposito> GetAll() {
        return _listaDepositos;
    }
    public Deposito? Find(Func<Deposito, bool> filtro) {
        return _listaDepositos.Where(filtro).FirstOrDefault();
    }
}