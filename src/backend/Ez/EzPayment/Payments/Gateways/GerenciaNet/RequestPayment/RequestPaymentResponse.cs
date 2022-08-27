using System;

namespace EzPayment.Payments.Gateways.GerenciaNet.RequestPayment
{
    public class RequestPaymentResponse
    {
        public string Txid { get; set; }
        public int Revisao { get; set; }
        public Loc Loc { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Chave { get; set; }
        public string SolicitacaoPagador { get; set; }
    }

    public class Loc
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string TipoCob { get; set; }
        public DateTime Criacao { get; set; }
    }
 
}
