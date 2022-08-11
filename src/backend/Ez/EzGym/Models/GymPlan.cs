using EzCommon.Models;

namespace EzGym.Models
{
    public class GymPlan : AggregateRoot
    {

        public string Name { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }

        protected override void RegisterEvents()
        {
            throw new System.NotImplementedException();
        }
    }
}
