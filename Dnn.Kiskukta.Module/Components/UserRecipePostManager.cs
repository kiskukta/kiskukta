using Dnn.Kiskukta.Dnn.Kiskukta.Module.Models;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dnn.Kiskukta.Dnn.Kiskukta.Module.Components
{
    public class UserRecipePostManager
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public UserRecipePostManager()
        {
            _connectionString = Config.GetConnectionString();
            _tableName = DataProvider.Instance().ObjectQualifier + "UserRecipePosts";
        }

        public List<UserRecipePostInfo> GetPosts(bool approvedOnly)
        {
            var posts = new List<UserRecipePostInfo>();

            var sql = @"
                SELECT urp.PostId, urp.ModuleId, urp.RecipeName, urp.CommentText, urp.ImagePath,
                       urp.CreatedByUserId, urp.CreatedByDisplayName, urp.CreatedOnDate,
                       urp.Status, urp.ProductBvin, pt.ProductName
                FROM " + _tableName + @" urp
                LEFT JOIN hcc_ProductTranslations pt
                    ON urp.ProductBvin = pt.ProductId";

            if (approvedOnly)
            {
                sql += " WHERE urp.Status = @Status";
            }

            sql += " ORDER BY urp.PostId DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (approvedOnly)
                {
                    cmd.Parameters.AddWithValue("@Status", "Approved");
                }

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        posts.Add(MapPost(reader));
                    }
                }
            }

            return posts;
        }

        public List<UserRecipePostInfo> GetPostsByProduct(string productBvin)
        {
            var posts = new List<UserRecipePostInfo>();

            var sql = @"
        SELECT urp.PostId, urp.ModuleId, urp.RecipeName, urp.CommentText, urp.ImagePath,
               urp.CreatedByUserId, urp.CreatedByDisplayName, urp.CreatedOnDate,
               urp.Status, urp.ProductBvin, pt.ProductName
        FROM " + _tableName + @" urp
        LEFT JOIN hcc_ProductTranslations pt
            ON urp.ProductBvin = pt.ProductId
        WHERE urp.Status = @Status
          AND urp.ProductBvin = @ProductBvin
        ORDER BY urp.PostId DESC";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Status", "Approved");
                cmd.Parameters.AddWithValue("@ProductBvin", Guid.Parse(productBvin));

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        posts.Add(MapPost(reader));
                    }
                }
            }

            return posts;
        }

        public List<ProductDropdownItem> GetProducts()
        {
            var products = new List<ProductDropdownItem>();

            var sql = @"
                SELECT DISTINCT p.bvin, pt.ProductName
                FROM hcc_Product p
                INNER JOIN hcc_ProductTranslations pt
                    ON p.bvin = pt.ProductId
                INNER JOIN hcc_ProductXCategory pc
                    ON p.bvin = pc.ProductId
                INNER JOIN hcc_Category c
                    ON pc.CategoryId = c.bvin
                INNER JOIN hcc_CategoryTranslations ct
                    ON c.bvin = ct.CategoryId
                WHERE ct.Name = 'Boxok'
                  AND pt.ProductName IS NOT NULL
                ORDER BY pt.ProductName";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new ProductDropdownItem
                        {
                            Bvin = reader["bvin"].ToString(),
                            ProductName = reader["ProductName"].ToString()
                        });
                    }
                }
            }

            return products;
        }

        public void CreatePost(UserRecipePostInfo postInfo)
        {
            var sql = @"
                INSERT INTO " + _tableName + @"
                (ModuleId, RecipeName, CommentText, ImagePath,
                 CreatedByUserId, CreatedByDisplayName, CreatedOnDate, Status, ProductBvin)
                VALUES
                (@ModuleId, @RecipeName, @CommentText, @ImagePath,
                 @CreatedByUserId, @CreatedByDisplayName, @CreatedOnDate, @Status, @ProductBvin)";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ModuleId", postInfo.ModuleId);
                cmd.Parameters.AddWithValue("@RecipeName", postInfo.RecipeName);
                cmd.Parameters.AddWithValue("@CommentText", postInfo.CommentText);
                cmd.Parameters.AddWithValue("@ImagePath", (object)postInfo.ImagePath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedByUserId", postInfo.CreatedByUserId);
                cmd.Parameters.AddWithValue("@CreatedByDisplayName", (object)postInfo.CreatedByDisplayName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedOnDate", postInfo.CreatedOnDate);
                cmd.Parameters.AddWithValue("@Status", postInfo.Status);
                cmd.Parameters.AddWithValue("@ProductBvin", Guid.Parse(postInfo.ProductBvin));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool UpdateStatus(int postId, string status)
        {
            var sql = @"
                UPDATE " + _tableName + @"
                SET Status = @Status
                WHERE PostId = @PostId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void DeletePost(int postId)
        {
            var sql = @"
                DELETE FROM " + _tableName + @"
                WHERE PostId = @PostId";

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PostId", postId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private UserRecipePostInfo MapPost(SqlDataReader reader)
        {
            return new UserRecipePostInfo
            {
                PostId = Null.SetNullInteger(reader["PostId"]),
                ModuleId = Null.SetNullInteger(reader["ModuleId"]),
                RecipeName = Null.SetNullString(reader["RecipeName"]),
                CommentText = Null.SetNullString(reader["CommentText"]),
                ImagePath = Null.SetNullString(reader["ImagePath"]),
                CreatedByUserId = Null.SetNullInteger(reader["CreatedByUserId"]),
                CreatedByDisplayName = Null.SetNullString(reader["CreatedByDisplayName"]),
                CreatedOnDate = Null.SetNullDateTime(reader["CreatedOnDate"]),
                Status = Null.SetNullString(reader["Status"]),
                ProductBvin = reader["ProductBvin"] == DBNull.Value ? null : reader["ProductBvin"].ToString(),
                ProductName = reader["ProductName"] == DBNull.Value ? null : reader["ProductName"].ToString()
            };
        }
    }
}