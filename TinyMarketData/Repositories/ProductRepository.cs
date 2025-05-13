using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;

namespace TinyMarketData.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// obtiene un listado de productos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<Product> GetAll()
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetProducts", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            List<Product> products = new List<Product>();
                            foreach (DataRow item in dt.Rows)
                            {
                                products.Add(this.GetProductEntity(item));
                            }
                            return products;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los Productos {ex.Message}");
            }
        }

        /// <summary>
        /// obtiene un listado de productos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<Product> GetProductsFiltered(ProductFiltered entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetProductsFiltered", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", entity.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", entity.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@MIN_PRICE", entity.MinPrice));
                        cmd.Parameters.Add(new SqlParameter("@MAX_PRICE", entity.MaxPrice));
                        cmd.Parameters.Add(new SqlParameter("@STATUS", entity.Status));
                        cmd.Parameters.Add(new SqlParameter("@STOCK", entity.Stock ?? 0));

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            List<Product> products = new List<Product>();
                            foreach (DataRow item in dt.Rows)
                            {
                                products.Add(this.GetProductEntity(item));
                            }
                            return products;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los Productos {ex.Message}");
            }
        }

        /// <summary>
        /// Agrega un nuevo producto
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Add(Product entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", entity.Description));
                        cmd.Parameters.Add(new SqlParameter("@PRICE", entity.Price));
                        cmd.Parameters.Add(new SqlParameter("@STOCK", entity.Stock));
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", entity.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", entity.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@EXPIRATION_DATE", entity.ExpirationDate));
                        cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", SqlDbType.Int) { Direction = ParameterDirection.Output });

                        cmd.ExecuteNonQuery();

                        return Convert.ToInt32(cmd.Parameters["@PRODUCT_ID"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar el Producto. {ex.Message}");
            }
        }

        /// <summary>
        /// Modifica un producto
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception"></exception>
        public void Update(Product entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", entity.ProductId));
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", entity.Description));
                        cmd.Parameters.Add(new SqlParameter("@PRICE", entity.Price));
                        cmd.Parameters.Add(new SqlParameter("@STOCK", entity.Stock));
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", entity.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("@SUPPLIER_ID", entity.SupplierId));
                        cmd.Parameters.Add(new SqlParameter("@EXPIRATION_DATE", entity.ExpirationDate));
                        cmd.Parameters.Add(new SqlParameter("@STATUS", entity?.Status));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar el producto. {ex.Message}");
            }
        }

        /// <summary>
        /// elimina un producto
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
                    using (SqlCommand cmd = new SqlCommand("DeleteProduct", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@PRODUCT_ID", id));

                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Producto. {ex.Message}");
            }
        }


        private Product GetProductEntity(DataRow item)
        {
            return new Product()
            {
                ProductId = Convert.ToInt32(item["PRODUCT_ID"]),
                Name = item["NAME"].ToString(),
                Description = item["DESCRIPTION"].ToString(),
                Price = Convert.ToDecimal(item["PRICE"]),
                Stock = Convert.ToInt32(item["STOCK"]),
                CategoryId = Convert.ToInt32(item["CATEGORY_ID"]),
                SupplierId = Convert.ToInt32(item["SUPPLIER_ID"]),
                CreationDate = item["CREATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["CREATION_DATE"]) : null,
                ModificationDate = item["MODIFICATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["MODIFICATION_DATE"]) : null,
                ExpirationDate = item["EXPIRATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["EXPIRATION_DATE"]) : null,
                Status = item["STATUS"].ToString()
            };
        }
    }
}