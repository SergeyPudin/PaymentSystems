using System.Security.Cryptography;
using System.Text;

namespace PaymentSystems
{
    internal interface IPaymentSystem
    {
        string GetPayingLink(Order order);
    }

    internal interface IHashCalculator
    {
        string CalculateHash(string input);
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var order = new Order(1, 12000);

            IHashCalculator md5Calculator = new MD5HashCalculator();
            IHashCalculator sha1Calculator = new SHA1HashCalculator();

            IPaymentSystem system1 = new PaymentSystem1(md5Calculator);
            IPaymentSystem system2 = new PaymentSystem2(md5Calculator);
            IPaymentSystem system3 = new PaymentSystem3(sha1Calculator, "SecretKey");


            Console.WriteLine(system1.GetPayingLink(order));
            Console.WriteLine(system2.GetPayingLink(order));
            Console.WriteLine(system3.GetPayingLink(order));
        }
    }

    internal class MD5HashCalculator : IHashCalculator
    {
        public string CalculateHash(string input)
        {
            if (input == null)
                throw new ArgumentNullException();

            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    internal class SHA1HashCalculator : IHashCalculator
    {
        public string CalculateHash(string input)
        {
            if (input == null)
                throw new ArgumentNullException();

            using var sha1 = SHA1.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha1.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

    }
    internal class Order
    {
        private int _id;
        private int _amount;

        public Order(int id, int amount)
        {
            if (id <= 0 || amount <= 0)
                throw new ArgumentOutOfRangeException();

            _id = id;
            _amount = amount;
        }

        public int Id => _id;
        public int Amount => _amount;
    }

    internal class PaymentSystem1 : IPaymentSystem
    {
        private IHashCalculator _hashCalculator;

        internal PaymentSystem1(IHashCalculator hashCalculator)
        {
            if(hashCalculator == null) 
                throw new ArgumentNullException();

            _hashCalculator = hashCalculator;
        }

        public string GetPayingLink(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            string hash = _hashCalculator.CalculateHash(order.Id.ToString());

            return $"pay.system1.ru/order?amount={order.Amount}RUB&hash={hash}";
        }
    }

    internal class PaymentSystem2 : IPaymentSystem
    {
        private IHashCalculator _hashCalculator;

        internal PaymentSystem2(IHashCalculator hashCalculator)
        {
            if(hashCalculator == null)
                throw new ArgumentNullException();

            _hashCalculator = hashCalculator;
        }

        public string GetPayingLink(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            string data = order.Id + order.Amount.ToString();
            string hash = _hashCalculator.CalculateHash(data.ToString());

            return $"order.system2.ru/pay?hash={hash}";
        }
    }

    internal class PaymentSystem3 : IPaymentSystem
    {
        private string _secretKey;
        private IHashCalculator _hashCalculator;

        internal PaymentSystem3(IHashCalculator hashCalculator, string secretKey)
        {
            if (secretKey == null || hashCalculator == null)
                throw new ArgumentNullException();

            _secretKey = secretKey;
            _hashCalculator = hashCalculator;
        }

        public string GetPayingLink(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            string data = $"{order.Amount}{order.Id}{_secretKey}";
            string hash = _hashCalculator.CalculateHash(data.ToString());

            return $"system3.com/pay?amount={order.Amount}&curency=RUB&hash={hash}";
        }
    }
}