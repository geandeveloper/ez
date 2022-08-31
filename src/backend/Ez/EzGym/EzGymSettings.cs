namespace EzGym;

public class EzGymSettings
{
    public Frontend Frontend { get; set; }
    public Storage Storage { get; set; }
}

public class Frontend
{
    public string EzGymWebUrl { get; set; }
    public string EzGymProductionWebUrl { get; set; }
}

public class Storage
{
    public Marten Marten { get; set; }
    public GCP GCP { get; set; }
}

public class Marten
{
    public string ConnectionString { get; set; }
}

public class GCP
{
    public string CredentialsPath { get; set; }
}

