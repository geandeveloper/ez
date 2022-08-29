using EzCommon.Commands;

namespace EzPayment.Payments.CreatePayment;

public record CreatePaymentCommand(long Amount, string Description, PaymentMethodEnum PaymentMethod) : ICommand;