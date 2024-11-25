using WebApi.Model;

namespace WebApi.Interface;

public interface ICompraService
{
    public Compra Add(Compra compra);
    public IEnumerable<Compra> GetALL();
    public Compra GetByID(int id);
    void Update(Compra compra);
    void Delete(int id);
}