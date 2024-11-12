using WebApi.Model;

namespace WebApi.Interface;

public interface IVentaService{
    public Venta Add(Venta venta);
    public IEnumerable<Venta> GetALL();
    public Venta GetByID(int id);
    void Update(Venta factura);
    void Delete(int id);
}


