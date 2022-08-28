namespace EzPayment;

public class EzPaymentSettings
{
    public GerenciaNet GerenciaNet { get; set; }
    public StripePayments StripePayments { get; set; }
    public Storage Storage { get; set; }
}

public class GerenciaNet 
{
    public string PixKey { get; set; }
}

public class StripePayments 
{
    public string ApiSecretKey { get; set; }
    public string WebhookSecret { get; set; }
}

public class Storage
{
    public Marten Marten { get; set; }
}

public class Marten
{
    public string ConnectionString { get; set; }
}

