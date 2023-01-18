using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Models;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AdminPortal.Components.Pages
{
    public partial class EditUserComponent
    {
        [Parameter]
        public AdminViewModel AdminViewModel { get; set; }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                //jsModule = await Js.InvokeAsync<IJSObjectReference>("~/js/Fields.js");
                await Js.InvokeVoidAsync("Fields");
            }
        }
    }
}