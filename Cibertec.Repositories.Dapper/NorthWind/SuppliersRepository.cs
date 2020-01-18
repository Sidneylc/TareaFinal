using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Cibertec.Models;
using Cibertec.Repositories.NorthWind;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Cibertec.Repositories.Dapper.NorthWind
{
    class SuppliersRepository : Repository<Suppliers>, ISuppliersRepository
    {
        public SuppliersRepository(string connectionString) : base(connectionString)
        {

        }

        public new Suppliers GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Suppliers>().Where(
                supplier => supplier.SupplierID.Equals(id)).First();
            }
        }

        public new bool Update(Suppliers supplier)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = connection.Execute("update Suppliers " +
                                            "set CompanyName = @company, " +
                                            "ContactName = @contact, " +
                                            "ContactTitle = @title, " +
                                            "City = @city " +
                                            "Country = @country, " +
                                            "Phone = @phone " +
                                            "where SupplierID = @id",
                                            new
                                            {
                                                company = supplier.CompanyName,
                                                contact = supplier.ContactName,
                                                title = supplier.ContactTitle,
                                                city = supplier.City,
                                                country = supplier.Country,
                                                phone = supplier.Phone,
                                                id = supplier.SupplierID
                                            });
                return Convert.ToBoolean(result);
            }

        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from Suppliers " +
                    "where SupplierID = @myid ", new { myid = id });

                return Convert.ToBoolean(result);
            }
        }

        public IEnumerable<Suppliers> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Suppliers>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Suppliers>("dbo.uspSupplierPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(SupplierID) from dbo.Suppliers");
            }
        }
    }
}