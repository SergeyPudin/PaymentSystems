namespace PaymentSystems
{
    internal interface IPaymentSystem
    {
        string GetPayingLink(Order order);
    }
}
