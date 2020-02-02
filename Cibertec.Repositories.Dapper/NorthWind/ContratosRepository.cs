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
    public class ContratosRepository : Repository<Contrato>, IContratosRepository
    {
        public ContratosRepository(string connectionString) : base(connectionString)
        {

        }

        public new Contrato GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetAll<Contrato>().Where(contrato => contrato.Codigo.Equals(id)).First();
            }
        }

        public new int Insert(Contrato contrato)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("insert Contrato (Fecha, Direccion, Indicaciones, Comentarios, CodCliente, CodServicio, CodLugar) " +
                                                "values (@fecha, @direccion, @indicaciones, @comentarios, @codCliente, @codServicio, @codLugar)",
                                            new
                                            {
                                                fecha = contrato.Fecha,
                                                direccion = contrato.Direccion,
                                                indicaciones = contrato.Indicaciones,
                                                comentarios = contrato.Comentarios,
                                                codCliente = contrato.CodCliente,
                                                codServicio = contrato.CodServicio,
                                                codLugar = contrato.CodLugar
                                            });
                return result;
            }
        }

        public new bool Update(Contrato contrato)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("update contrato " +
                        "set Fecha = @fecha, Direccion = @direccion, Indicaciones = @indicaciones, Comentarios = @comentarios, CodCliente = @codCliente, CodServicio = @codServicio, CodLugar = @codLugar" +
                                            "where Codigo = @codigo",
                                            new
                                            {
                                                fecha = contrato.Fecha,
                                                direccion = contrato.Direccion,
                                                indicaciones = contrato.Indicaciones,
                                                comentarios = contrato.Comentarios,
                                                codCliente = contrato.CodCliente,
                                                codServicio = contrato.CodServicio,
                                                codLugar = contrato.CodLugar
                                            });
                return Convert.ToBoolean(result);
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Execute("delete from contrato " +
                    "where Codigo = @myid ", new { myid = id });
                return Convert.ToBoolean(result);
                //if (result==1) return true; else return false;   
            }
        }

        public IEnumerable<Contrato> PagedList(int startRow, int endRow)
        {
            if (startRow >= endRow) return new List<Contrato>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@startRow", startRow);
                parameters.Add("@endRow", endRow);
                return connection.Query<Contrato>("dbo.uspContratoPagedList", parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int Count()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.ExecuteScalar<int>("Select Count(Codigo) from dbo.Contrato");
            }
        }
    }
}