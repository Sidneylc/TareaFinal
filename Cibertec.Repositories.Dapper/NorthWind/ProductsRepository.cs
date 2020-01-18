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
    class ProductsRepository : Repository<Products>, IProductsRepository
    {
        public ProductsRepository(string connectionString) : base(connectionString)
        {

        }

        public Products GetById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Products>().Where(
                product => product.ProductID.Equals(id)).First();
            }
        }

        public new bool Update(Products product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var result = connection.Execute("update Products " +
                                            "set ProductName = @name, " +
                                            "UnitPrice = @price, " +
                                            "UnitsInStock = @units, " +
                                            "IsDiscontinued = @disc " +
                                            "where ProductID = @id",
                                            new
                                            {
                                                name = product.ProductName,
                                                price = product.UnitPrice,
                                                units = product.UnitsInStock,
                                                disc = product.IsDiscontinued,
                                                id = product.ProductID
                                            });
                return Convert.ToBoolean(result);
            }

        }

        public bool Delete(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from Products " +
                    "where ProductID = @myid ", new { myid = id });

                return Convert.ToBoolean(result);
            }
        }

        public IEnumerable<Products> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Products>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Products>("dbo.uspProductPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(ProductID) from dbo.Products");
            }
        }
    }
}