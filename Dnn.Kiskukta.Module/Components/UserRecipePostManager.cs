using Dnn.Kiskukta.Dnn.Kiskukta.Module.Models;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
            //var posts = new List<UserRecipePostInfo>();

            //var sql = @"
            //    SELECT PostId, ModuleId, RecipeName, CommentText, ImagePath,
            //           CreatedByUserId, CreatedByDisplayName, CreatedOnDate, Status
            //    FROM " + _tableName + @"
            //    WHERE ModuleId = @ModuleId";

            //if (approvedOnly)
            //{
            //    sql += " AND Status = @Status";
            //}

            //sql += " ORDER BY CreatedOnDate DESC, PostId DESC";

            //using (var conn = new SqlConnection(_connectionString))
            //using (var cmd = new SqlCommand(sql, conn))
            //{
            //    cmd.Parameters.AddWithValue("@ModuleId", moduleId);

            //    if (approvedOnly)
            //    {
            //        cmd.Parameters.AddWithValue("@Status", "Approved");
            //    }

            //    conn.Open();

            //    using (var reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            posts.Add(MapPost(reader));
            //        }
            //    }
            //}

            //return posts;

            var posts = new List<UserRecipePostInfo>();

            var sql = @"
        SELECT PostId, ModuleId, RecipeName, CommentText, ImagePath,
               CreatedByUserId, CreatedByDisplayName, CreatedOnDate, Status, Category
        FROM " + _tableName;

            if (approvedOnly)
            {
                sql += " WHERE Status = @Status";
            }

            sql += " ORDER BY CreatedOnDate DESC, PostId DESC";

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

        public void CreatePost(UserRecipePostInfo postInfo)
        {
            var sql = @"
                INSERT INTO " + _tableName + @"
                (ModuleId, RecipeName, CommentText, ImagePath,
                 CreatedByUserId, CreatedByDisplayName, CreatedOnDate, Status, Category)
                VALUES
                (@ModuleId, @RecipeName, @CommentText, @ImagePath,
                 @CreatedByUserId, @CreatedByDisplayName, @CreatedOnDate, @Status, @Category)";

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
                cmd.Parameters.AddWithValue("@Category", (object)postInfo.Category ?? DBNull.Value); 

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
                Category = Null.SetNullString(reader["Category"])
            };
        }
    }
}