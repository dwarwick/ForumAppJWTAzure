using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.Models
{
    public class UploadFileModel
    {
        public byte[]? Data { get; set; }
        public string? FileName { get; set; }
        public int PostId { get; set; }
    }
}
