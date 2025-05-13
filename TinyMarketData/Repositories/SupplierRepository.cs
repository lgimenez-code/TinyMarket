using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketData.Repositories
{
    public class SupplierRepository : BaseRepository, ISupplierRepository
    {
        public SupplierRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// obtiene un listado de proveedores
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<Supplier> GetAll()
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetSuppliers", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            List<Supplier> products = new List<Supplier>();
                            foreach (DataRow item in dt.Rows)
                            {
                                products.Add(this.GetSupplierEntity(item));
                            }
                            return products;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los Proveedores {ex.Message}");
            }
        }

        /// <summary>
        /// crea un nuevo proveedor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Add(Supplier entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@ADDRESS", entity.Address));
                        cmd.Parameters.Add(new SqlParameter("@PHONE", entity.Phone));
                        cmd.Parameters.Add(new SqlParameter("@EMAIL", entity.Email));
                        cmd.Parameters.Add(new SqlParameter("@PROVINCE_ID", entity.ProvinceId));
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", SqlDbType.Int) { Direction = ParameterDirection.Output });

                        cmd.ExecuteNonQuery();

                        return Convert.ToInt32(cmd.Parameters["@SUPPLIER_ID"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar el Proveedor. {ex.Message}");
            }
        }

        /// <summary>
        /// modifica un proveedor
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public void Update(Supplier entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", entity.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@ADDRESS", entity.Address));
                        cmd.Parameters.Add(new SqlParameter("@PHONE", entity.Phone));
                        cmd.Parameters.Add(new SqlParameter("@EMAIL", entity.Email));
                        cmd.Parameters.Add(new SqlParameter("@PROVINCE_ID", entity.ProvinceId));
                        cmd.Parameters.Add(new SqlParameter("@STATUS", entity.Status));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Proveedor. {ex.Message}");
            }
        }

        /// <summary>
        /// elimina un proveedor
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(int id)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DeleteSupplier", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", id));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Proveedor. {ex.Message}");
            }
        }


        private Supplier GetSupplierEntity(DataRow item)
        {
            return new Supplier()
            {
                SupplierId = item["SUPPLIER_ID"] != DBNull.Value ? Convert.ToInt32(item["SUPPLIER_ID"]) : null,
                Name = item["NAME"].ToString(),
                Address = item["ADDRESS"].ToString(),
                Phone = item["PHONE"].ToString(),
                Email = item["EMAIL"].ToString(),
                ProvinceId = item["PROVINCE_ID"] != DBNull.Value ? Convert.ToInt32(item["PROVINCE_ID"]) : null,
                CreationDate = item["CREATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["CREATION_DATE"]) : null,
                ModificationDate = item["MODIFICATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["MODIFICATION_DATE"]) : null,
                Status = item["STATUS"].ToString()
            };
        }
    }
}
