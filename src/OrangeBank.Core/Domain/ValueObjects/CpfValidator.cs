namespace OrangeBank.Core.Domain.ValueObjects;

public static class CpfValidator
{
    public static bool IsValid(string cpf)
    {
        // Remove caracteres não numéricos
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Cálculo do primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (10 - i) * (cpf[i] - '0');

        int remainder = sum % 11;
        int digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (cpf[9] - '0' != digit1)
            return false;

        // Cálculo do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (11 - i) * (cpf[i] - '0');

        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf[10] - '0' == digit2;
    }
}