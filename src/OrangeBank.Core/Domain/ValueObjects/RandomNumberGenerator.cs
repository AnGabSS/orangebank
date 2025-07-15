public class RandomNumberGenerator
{
    private static readonly Random _random = new Random();

    public static string GenerateRandom8DigitNumber()
    {
        int randomNumber = _random.Next(10000000, 99999999 + 1);
        return randomNumber.ToString();
    }

    public static string GenerateRandom16DigitNumber()
    {
        long randomNumber = _random.NextInt64(1000000000000000, 9999999999999999 + 1);
        return randomNumber.ToString();
    }
}