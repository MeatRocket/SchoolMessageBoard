using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminPortal.Hubs;
using AdminPortal.Services;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AdminPortal.Components.Pages
{
    public partial class CommentComponent
    {
        [Parameter]
        public string PostId { get; set; }
        [Parameter]
        public string UserId { get; set; }
        public Comment Comment { get; set; } = new();

        private static Random random = new();

        private HubConnection _hubConnection;
        private string _hubUrl { get; set; }

        private List<string> Boarders = new() { "border-primary", "border-success", "border-danger", "border-warning", "border-info", "border-white" };

        protected override async Task OnInitializedAsync()
        {
            var x = _dbContext;
            CommentServices.PostId = PostId;
            CommentServices.UserId = UserId;
            CommentServices.Comments = _dbContext.Comments.Include(x => x.User).Where(x => x.CommentPostId == PostId).ToList();
            CommentServices._dbcontext = _dbContext;

            _hubUrl = navigationManager.BaseUri.TrimEnd('/') + CommentHub.CommentUrl;
            _hubConnection = new HubConnectionBuilder().WithUrl(navigationManager.ToAbsoluteUri("/DynamicPostView")).Build();

            _hubConnection.On("Broadcast", UpdateComments);
            _hubConnection.On("ReceiveMessage", () =>
                        {
                            InvokeAsync(StateHasChanged);
                        });

            await _hubConnection.StartAsync();

            await base.OnInitializedAsync();
            CommentServices.MyEventHandler += () => InvokeAsync(() => StateHasChanged());
        }


        public async Task UpdateComments()
        {
            CommentServices.PostId = PostId;
            CommentServices.UserId = UserId;
            await InvokeAsync(StateHasChanged);
            await CommentServices.UpdateComments();
            await _hubConnection.SendAsync("Broadcast");

        }
        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}