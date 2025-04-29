public static class ValidationHelper 
{
    public static bool IsValidInput(string input, string allowedSpecialCharacters = "")
    {
        if (string.IsNullOrEmpty(input)) return false;

        var validCharacters = allowedSpecialCharacters.ToHashSet();

        return input.All(c => char.IsLetterOrDigit(c) || validCharacters.Contains(c));
    }

    public static bool IsValidXSSInput(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;

        string lower = input.ToLower();
        if (lower.Contains("<script") || lower.Contains("<iframe"))
        {
            return false;
        }

        return true;
    }
}