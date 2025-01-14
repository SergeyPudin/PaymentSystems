using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    public class PaymentSystem1 : IPaymentSystem
    {

        public string GetPayingLink(Order order)
        {
            string hash = CalculateMD5(order.Id.ToString());
            return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={hash}";
        }

        public string CalculateMD5(string input)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}