﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class AppLogViewModel : BaseViewModel
    {
        public string? Project { get; set; }
        public string? FileName { get; set; }
        public string? Method { get; set; }
        public string? Message { get; set; }
        public string? Severity { get; set; }
    }
}
