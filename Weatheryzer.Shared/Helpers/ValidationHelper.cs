namespace Weatheryzer.Shared;

public static class ValidationHelper
{
    public static bool ValidateNotNull<T>(T obj, string paramName) where T : class
    {
        if (obj == null)
            throw new ArgumentNullException(paramName);

        return true;
    }
}