namespace EzGym.Payments.Gateways.GerenciaNet.RequestPayment
{
    public class RequestPaymentRequest
    {
        public Calendario Calendario { get; set; }
        public Valor Valor { get; set; }
        public string Chave { get; set; }
        public string SolicitacaoPagador { get; set; }
    }

    public class Calendario
    {
        public int Expiracao { get; set; }
    }

    public class Valor
    {
        public string Original { get; set; }
    }
}
