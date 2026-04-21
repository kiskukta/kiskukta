using DotNetNuke.Common.Utilities;
using Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Components
{
    public class UserRecipePostManager
    {
        private readonly string _connectionString;

        public UserRecipePostManager()
        {
            _connectionString = Config.GetConnectionString();
        }

        public List<UserRecipePostInfo> GetPosts(int moduleId)
        {
            var posts = new List<UserRecipePostInfo>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT PostId, ModuleId, RecipeName, CommentText, ImagePath, CreatedByUserId, CreatedOnDate, Status
                FROM UserRecipePosts
                WHERE ModuleId = @ModuleId
                ORDER BY CreatedOnDate DESC", conn))
            {
                cmd.Parameters.AddWithValue("@ModuleId", moduleId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        posts.Add(new UserRecipePostInfo
                        {
                            PostId = Null.SetNullInteger(reader["PostId"]),
                            ModuleId = Null.SetNullInteger(reader["ModuleId"]),
                            RecipeName = Null.SetNullString(reader["RecipeName"]),
                            CommentText = Null.SetNullString(reader["CommentText"]),
                            ImagePath = Null.SetNullString(reader["ImagePath"]),
                            CreatedByUserId = Null.SetNullInteger(reader["CreatedByUserId"]),
                            CreatedOnDate = Null.SetNullDateTime(reader["CreatedOnDate"]),
                            Status = Null.SetNullString(reader["Status"])
                        });
                    }
                }
            }

            return posts;
        }

        public UserRecipePostInfo GetPost(int postId)
        {
            UserRecipePostInfo post = null;

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT PostId, ModuleId, RecipeName, CommentText, ImagePath, CreatedByUserId, CreatedOnDate, Status
                FROM UserRecipePosts
                WHERE PostId = @PostId", conn))
            {
                cmd.Parameters.AddWithValue("@PostId", postId);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        post = new UserRecipePostInfo
                        {
                            PostId = Null.SetNullInteger(reader["PostId"]),
                            ModuleId = Null.SetNullInteger(reader["ModuleId"]),
                            RecipeName = Null.SetNullString(reader["RecipeName"]),
                            CommentText = Null.SetNullString(reader["CommentText"]),
                            ImagePath = Null.SetNullString(reader["ImagePath"]),
                            CreatedByUserId = Null.SetNullInteger(reader["CreatedByUserId"]),
                            CreatedOnDate = Null.SetNullDateTime(reader["CreatedOnDate"]),
                            Status = Null.SetNullString(reader["Status"])
                        };
                    }
                }
            }

            return post;
        }

        public void CreatePost(UserRecipePostInfo postInfo)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                INSERT INTO UserRecipePosts
                (ModuleId, RecipeName, CommentText, ImagePath, CreatedByUserId, CreatedOnDate, Status)
                VALUES
                (@ModuleId, @RecipeName, @CommentText, @ImagePath, @CreatedByUserId, @CreatedOnDate, @Status)", conn))
            {
                cmd.Parameters.AddWithValue("@ModuleId", postInfo.ModuleId);
                cmd.Parameters.AddWithValue("@RecipeName", postInfo.RecipeName);
                cmd.Parameters.AddWithValue("@CommentText", postInfo.CommentText);
                cmd.Parameters.AddWithValue("@ImagePath", (object)postInfo.ImagePath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedByUserId", postInfo.CreatedByUserId);
                cmd.Parameters.AddWithValue("@CreatedOnDate", postInfo.CreatedOnDate);
                cmd.Parameters.AddWithValue("@Status", postInfo.Status);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool UpdateStatus(int postId, string status)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        UPDATE UserRecipePosts
        SET Status = @Status
        WHERE PostId = @PostId", conn))
            {
                cmd.Parameters.AddWithValue("@PostId", postId);
                cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public void DeletePost(int postId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM UserRecipePosts WHERE PostId = @PostId", conn))
            {
                cmd.Parameters.AddWithValue("@PostId", postId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}