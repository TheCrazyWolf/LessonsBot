
using System.ComponentModel.DataAnnotations;

namespace LessonsBot_DB.ModelsDb
{
    public class Bot
    {
        [Key]
        public Guid IdBot { get; set; }
        public string Token { get; set; }
        public long? IdValueService { get; set; }
        public int TimeOutResponce { get; set; }
        public List<PeerProp> PeerProps { get; set; }

        public Bot()
        {
            PeerProps = new();
        }
    }
}
