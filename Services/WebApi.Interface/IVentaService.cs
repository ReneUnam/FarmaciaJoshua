using WebApi.Model;

namespace WebApi.Interface;

public interface IVentaService{
    public Venta Add(Venta venta);
    string ObtenerProximoNumeroFactura();
    public IEnumerable<Venta> GetALL();
    public Venta GetByID(int id);
    void Update(Venta venta);
    void Delete(int id);
}


