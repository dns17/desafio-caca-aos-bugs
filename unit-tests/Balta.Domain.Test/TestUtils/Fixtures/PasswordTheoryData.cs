using Balta.Domain.Test.TestUtils.Constaints;

namespace Balta.Domain.Test.TestUtils.Fixtures;

public static class PasswordTheoryData
{
    public static TheoryData<int> SpecialCharsPositions()
    {
        TheoryData<int> charsPositions = new TheoryData<int>();
        var combinations = Enumerable.Range(0, Constaint.Password.SpecialChars.Length);

        foreach (var combination in combinations.OrderBy(x => x))
            charsPositions.Add(combination);

        return charsPositions;
    }
}