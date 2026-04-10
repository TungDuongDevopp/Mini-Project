public static class InputHelper
{
    public static T Input<T>(
        string prompt,
        Func<string, (bool success, T value)> parser,
        Func<T, bool>? validator = null)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Không được để trống!");
                continue;
            }

            var (success, value) = parser(input);

            if (!success)
            {
                Console.WriteLine("❌ Sai định dạng!");
                continue;
            }

            if (validator != null && !validator(value))
            {
                Console.WriteLine("❌ Giá trị không hợp lệ!");
                continue;
            }

            return value;
        }
    }
    public static class Parsers
    {
        public static (bool, int) Int(string input)
            => (int.TryParse(input, out var v), v);

        public static (bool, decimal) Decimal(string input)
            => (decimal.TryParse(input, out var v), v);

        public static (bool, string) String(string input)
            => (!string.IsNullOrWhiteSpace(input), input);
        public static (bool, DateTime) Date(string input)
    => (DateTime.TryParse(input, out var v), v);
    }
}