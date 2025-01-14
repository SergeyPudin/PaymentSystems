using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    public class PaymentSystem2 : IPaymentSystem
    {
        public string GetPayingLink(Order order)
        {
            string data = order.Id + order.Amount.ToString();
            string hash = CalculateMD5(data);
            return $"order.system2.ru/pay?hash={hash}";
        }

        private string CalculateMD5(string input)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}