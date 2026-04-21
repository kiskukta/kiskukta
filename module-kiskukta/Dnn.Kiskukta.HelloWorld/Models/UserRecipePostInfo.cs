using System;

namespace Kiskukta.Dnn.Dnn.Kiskukta.HelloWorld.Models
{
    public class UserRecipePostInfo
    {
        public int PostId { get; set; }

        public int ModuleId { get; set; }

        public string RecipeName { get; set; }

        public string CommentText { get; set; }

        public string ImagePath { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public string Status { get; set; }
    }
}