using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Models;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AdminPortal.Views.Components
{
    public partial class EditUserComponent
    {
        [Parameter]
        [JsonIgnore]
        public AdminViewModel AdminViewModel { get; set; }
    }
}