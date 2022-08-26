namespace EzIdentity
{
    public class EzIdentitySettings
    {
        public string TokenSecurityKey { get; set; }
        public string EzIdentityUrl { get; set; }
        public Storage Storage { get; set; }
    }
    public class Storage
    {
        public Marten Marten { get; set; }
    }

    public class Marten
    {
        public string ConnectionString { get; set; }
    }
}
