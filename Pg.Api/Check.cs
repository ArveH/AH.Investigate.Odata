using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Pg.Api;

public static class Check
{
    public static T NotNull<T>([AllowNull] [NotNull] T value, string parameterName)
    {
        if (value is null)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    public static string NotEmpty([NotNull] string? value, string parameterName)
    {
        if (value is null)
        {
            NotEmpty(parameterName, nameof(parameterName));
            throw new ArgumentNullException(parameterName);
        }

        if (value.Trim().Length == 0)
        {
            NotEmpty(parameterName, nameof(parameterName));
            throw new ArgumentException(AbstractionsStrings.ArgumentIsEmpty(parameterName));
        }

        return value;
    }

    public static void DebugAssert([DoesNotReturnIf(false)] bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Check.DebugAssert failed: {message}");
        }
    }
}