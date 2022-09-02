namespace EzGym.Accounts
{
    public class Profile
    {
        public string Name { get; }
        public string JobDescription { get; }
        public string BioDescription { get; }

        private Profile() { }

        public Profile(string name, string jobDescription, string bioDescription)
        {
            Name = name;
            JobDescription = jobDescription;
            BioDescription = bioDescription;
        }
    }
}
