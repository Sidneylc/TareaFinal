using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cibertec.Repositories.Dapper.Cibertec;
using Cibertec.Repositories.NorthWind;
using Cibertec.UnitOfWork;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    public class NorthwindUnitOfWork : IUnitOfWork
    {
        public NorthwindUnitOfWork(string connectionString)
        {
            Customers = new CustomersRepository(connectionString);
            Orders = new OrdersRepository(connectionString);
            OrderDetails = new OrderDetailsRepository(connectionString);
            Users = new UserRepository(connectionString);
            Products = new ProductsRepository(connectionString);
            Suppliers = new SuppliersRepository(connectionString);

            Clientes = new ClientesRepository(connectionString);
            Contratistas = new ContratistasRepository(connectionString);
            Contratos = new ContratosRepository(connectionString);
            Lugares = new LugaresRepository(connectionString);
            Servicios = new ServiciosRepository(connectionString);
        }

        public ICustomersRepository Customers { get; private set; }
        public IOrdersRepository Orders { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IUserRepository Users { get; private set; }
        public IProductsRepository Products { get; private set; }
        public ISuppliersRepository Suppliers { get; private set; }

        public IClientesRepository Clientes { get; private set; }
        public IContratistasRepository Contratistas { get; private set; }
        public IContratosRepository Contratos { get; private set; }
        public ILugaresRepository Lugares { get; private set; }
        public IServiciosRepository Servicios { get; private set; }
    }
}
