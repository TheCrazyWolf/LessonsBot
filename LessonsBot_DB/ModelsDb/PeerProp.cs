using LessonsBot_DB.ModelService;
using System.ComponentModel.DataAnnotations;

namespace LessonsBot_DB.ModelsDb
{
    public class PeerProp
    {
        [Key]
        public Guid IdPeerProp { get; set; }
        public long IdPeer { get; set; }
        public TypeLesson TypeLesson { get; set; }
        public string Value { get; set; }

        public string? LastResult { get; set; }
    }
}
