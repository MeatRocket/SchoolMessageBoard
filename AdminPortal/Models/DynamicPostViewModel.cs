using MessageBoardClassLibrary.Models;

namespace AdminPortal.Models
{
    public class DynamicPostViewModel
    {
        public Dictionary<string, string> Types { get; set; } = new();
        public List<string> ErrorMessages { get; set; } = new();
        public List<Template> Templates { get; set; } = new(); 

    }
}
