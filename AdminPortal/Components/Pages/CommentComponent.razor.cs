using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AdminPortal.Components.Pages
{
    public partial class CommentComponent
    {
        [Parameter]
        public string PostId { get; set; }
        private static BoardContext _dbcontext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _dbcontext = DbFactory.CreateDbContext();
            if(_dbcontext is not null)
            {
                CommentServices.Comments = _dbcontext.Comments.Where(x => x.PostId == PostId).ToList();
            }

            CommentServices.MyEventHandler += () => InvokeAsync(StateHasChanged);
            CommentServices._dbcontext = _dbcontext;
            await base.OnInitializedAsync();
        }


        public void Dispose()
        {
            _dbcontext?.Dispose();
        }
    }
}