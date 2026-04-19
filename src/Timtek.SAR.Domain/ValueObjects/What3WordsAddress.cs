namespace Timtek.SAR.Domain.ValueObjects;

public sealed record What3WordsAddress
{
    public string Word1 { get; }
    public string Word2 { get; }
    public string Word3 { get; }

    public What3WordsAddress(string Word1, string Word2, string Word3)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Word1);
        ArgumentException.ThrowIfNullOrWhiteSpace(Word2);
        ArgumentException.ThrowIfNullOrWhiteSpace(Word3);

        this.Word1 = Word1;
        this.Word2 = Word2;
        this.Word3 = Word3;
    }

    public override string ToString() => $"///{Word1}.{Word2}.{Word3}";

    public static What3WordsAddress Parse(string input)
    {
        var trimmed = input.StartsWith("///") ? input[3..] : input;
        var parts = trimmed.Split('.');

        if (parts.Length != 3)
            throw new FormatException($"A What3Words address must contain exactly three words separated by dots. Got: '{input}'");

        return new What3WordsAddress(parts[0], parts[1], parts[2]);
    }
}
