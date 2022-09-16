using EzCommon.Models;

namespace EzGym.Players
{
    public class Player : AggregateRoot
    {
        public string AccountId { get; set; }
        public int Level { get; set; }


        public Player(string accountId)
        {
            
        }
    }
}
