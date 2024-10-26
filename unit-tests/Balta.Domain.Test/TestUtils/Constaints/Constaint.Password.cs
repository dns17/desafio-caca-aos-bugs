using System.Runtime.InteropServices;

namespace Balta.Domain.Test.TestUtils.Constaints;

public partial class Constaint
{
    public static class Password
    {
        public const int MaxLength = 49;
        public const int LengthHash = 16;
        public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const string SpecialChars = "!@#$%ˆ&*(){}[];";
        public static string CharSet => ValidChars + SpecialChars;
    }
}