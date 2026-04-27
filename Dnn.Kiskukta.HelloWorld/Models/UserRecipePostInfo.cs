using System;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Models
{
    [TableName("UserRecipePosts")]
    [PrimaryKey("PostId", AutoIncrement = true)]
    [Scope("ModuleId")]
    public class UserRecipePostInfo
    {
        public int PostId { get; set; }

        public int ModuleId { get; set; }

        public string RecipeName { get; set; }

        public string CommentText { get; set; }

        public string ImagePath { get; set; }

        public int CreatedByUserId { get; set; }

        public string CreatedByDisplayName { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public string Status { get; set; }

        [IgnoreColumn]
        public string StatusDisplay
        {
            get
            {
                switch ((Status ?? "").Trim().ToLower())
                {
                    case "approved":
                        return "Elfogadva";
                    case "pending":
                        return "Függőben";
                    case "rejected":
                        return "Elutasítva";
                    default:
                        return Status;
                }
            }
        }
    }
}