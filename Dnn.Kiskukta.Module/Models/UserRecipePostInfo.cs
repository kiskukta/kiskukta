using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Dnn.Kiskukta.Dnn.Kiskukta.Module.Models
{
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

        public string ProductBvin { get; set; }
        public string ProductName { get; set; }

        [IgnoreColumn]
        public List<ProductDropdownItem> Products { get; set; }

        [IgnoreColumn]
        public string StatusDisplay
        {
            get
            {
                switch ((Status ?? "").Trim().ToLower())
                {
                    case "approved":
                        return "Megjelenítve";
                    case "pending":
                        return "Függőben";
                    case "rejected":
                        return "Elrejtve";
                    default:
                        return Status;
                }
            }
        }
    }

    public class ProductDropdownItem
    {
        public string Bvin { get; set; }
        public string ProductName { get; set; }
    }
}