using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    public class PaymentSystem3 : IPaymentSystem
    {
        private readonly string _secretKey;

        public PaymentSystem3(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GetPayingLink(Order order)
        {
            string data = $"{order.Amount}{order.Id}{_secretKey}";
            string hash = CalculateSHA1(data);
            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={hash}";
        }

        private string CalculateSHA1(string input)
        {
            using var sha1 = SHA1.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}