namespace EzGym.Models
{
    public class Profile
    {
        public string Name { get; private set; }
        public string JobDescription { get; private set; }
        public string BioDescription { get; private set; }

        private Profile() { }

        public Profile(string name, string jobDescription, string bioDescription)
        {
            Name = name;
            JobDescription = jobDescription;
            BioDescription = bioDescription;
        }
    }
}
