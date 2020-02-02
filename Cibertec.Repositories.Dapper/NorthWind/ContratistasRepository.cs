using Dapper;
using Dapper.Contrib.Extensions;
using Cibertec.Models;
using Cibertec.Repositories.NorthWind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Cibertec.Repositories.Dapper.Cibertec
{
    public class ContratistasRepository : Repository<Contratista>, IContratistasRepository
    {
        public ContratistasRepository(string connectionString) : base(connectionString)
        {

        }

        public new Contratista GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Contratista>().Where(contratista => contratista.Codigo.Equals(id)).First();
            }
        }

        public new int Insert(Contratista contratista)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("insert contratista (Nombre, Apellido, Dni, Telefono, Correo) " +
                                                "values (@nombre, @apellido, @dni, @telefono, @correo)",
                                            new
                                            {
                                                nombre = contratista.Nombre,
                                                apellido = contratista.Apellido,
                                                dni = contratista.Dni,
                                                telefono = contratista.Telefono,
                                                correo = contratista.Correo
                                            });
                return result;
            }
        }

        public new bool Update(Contratista contratista)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("update contratista " +
                        "set Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono = @telefono, Correo = @correo" +
                                            "where Codigo = @codigo",
                                            new
                                            {
                                                nombre = contratista.Nombre,
                                                apellido = contratista.Apellido,
                                                dni = contratista.Dni,
                                                telefono = contratista.Telefono,
                                                correo = contratista.Correo
                                            });
                return Convert.ToBoolean(result);
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from contratista " +
                    "where Codigo = @myid ", new { myid = id });
                return Convert.ToBoolean(result);
                //if (result==1) return true; else return false;   
            }
        }

        public IEnumerable<Contratista> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Contratista>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Contratista>("dbo.uspContratistaPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(Codigo) from dbo.Contratista");
            }
        }
    }
}