using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.Models
{
    public class EmailTemplateModel
    {
        public string? product_url { get; set; }
        public string? product_name { get;set; }
        public string? name { get; set; }
        public string? action_url { get; set; }
        public string? action_name { get; set; }
        public string? company_name { get; set; }
        public string? company_address { get;set; }
        
    }
}
