namespace PaymentSystems
{
    class Program
    {
        static void Main(string[] args)
        {
            IPaymentSystem system1 = new PaymentSystem1();
            IPaymentSystem system2 = new PaymentSystem2();
            IPaymentSystem system3 = new PaymentSystem3("SecretKey");

            var order = new Order(1, 12000);

            Console.WriteLine(system1.GetPayingLink(order));
            Console.WriteLine(system2.GetPayingLink(order));
            Console.WriteLine(system3.GetPayingLink(order));
        }
    }
}