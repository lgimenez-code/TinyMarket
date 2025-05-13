using TinyMarketCore.Entities;
using TinyMarketCore.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TinyMarketData.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// obtiene un listado de categorias
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<Category> GetAll()
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetCategories", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            List<Category> categories = new List<Category>();
                            foreach (DataRow item in dt.Rows)
                            {
                                categories.Add(
                                    new Category()
                                    {
                                        CategoryId = Convert.ToInt32(item["CATEGORY_ID"]),
                                        Name = item["NAME"].ToString(),
                                        Description = item["DESCRIPTION"].ToString(),
                                        CreationDate = item["CREATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["CREATION_DATE"]) : null,
                                        ModificationDate = item["MODIFICATION_DATE"] != DBNull.Value ? Convert.ToDateTime(item["MODIFICATION_DATE"]) : null,
                                        Status = item["STATUS"].ToString()
                                    }
                                );
                            }
                            return categories;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las categorías {ex.Message}");
            }
        }

        /// <summary>
        /// registra una nueva Categoria
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int Add(Category entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("InsertCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", entity.Description));
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", SqlDbType.Int) { Direction = ParameterDirection.Output });

                        cmd.ExecuteNonQuery();

                        return Convert.ToInt32(cmd.Parameters["@CATEGORY_ID"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar la categoría. {ex.Message}");
            }
        }

        /// <summary>
        /// modifica una Categoria
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="Exception"></exception>
        public void Update(Category entity)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", entity.CategoryId));
                        cmd.Parameters.Add(new SqlParameter("@NAME", entity.Name));
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPTION", entity.Description));
                        cmd.Parameters.Add(new SqlParameter("@STATUS", entity?.Status ?? "R"));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al modificar la categoría. {ex.Message}");
            }
        }

        /// <summary>
        /// elimina una categoria
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

                    using (SqlCommand cmd = new SqlCommand("DeleteCategory", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", id));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la categoría. {ex.Message}");
            }
        }
    }
}
