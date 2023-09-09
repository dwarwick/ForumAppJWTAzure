using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumAppJWTAzure.Shared.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        public string Message { get; set; } = string.Empty;

        public string Target { get; set; } = string.Empty;

        public bool Read { get; set; }
    }
}
