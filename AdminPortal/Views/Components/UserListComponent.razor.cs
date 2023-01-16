using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using AdminPortal.Models;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace AdminPortal.Views.Components
{
    public partial class UserListComponent
    {
        [Parameter]
        [JsonIgnore]
        public List<User> Users { get; set; }
        [Parameter]
        public int TotalSize { get; set; }
    }
}