using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.Test.TestUtils;
using Balta.Domain.Test.TestUtils.Constaints;
using Balta.Domain.Test.TestUtils.Fixtures;
using FluentAssertions;
using Moq;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class PasswordTests
{
    private Mock<IRandomProvider> _randomProviderMock;

    public PasswordTests()
    {
        _randomProviderMock = new Mock<IRandomProvider>();
    }

    [Fact]
    public void ShouldFailIfPasswordIsNull()
    {
        // Arrange
        string? plainText = null;

        // Act
        Action action = () => Password.ShouldCreate(plainText!);

        // Assert
        action.Should().ThrowExactly<InvalidPasswordException>()
            .And.Message.Should().Be("Password cannot be null or empty");
    }

    [Fact]
    public void ShouldFailIfPasswordIsEmpty()
    {
        // Arrange
        string? plainText = "";

        // Act
        Action action = () => Password.ShouldCreate(plainText!);

        // Assert
        action.Should().ThrowExactly<InvalidPasswordException>()
            .And.Message.Should().Be("Password cannot be null or empty");
    }

    [Fact]
    public void ShouldFailIfPasswordIsWhiteSpace()
    {
        // Arrange
        string? plainText = "   ";

        // Act
        Action action = () => Password.ShouldCreate(plainText!);

        // Assert
        action.Should().ThrowExactly<InvalidPasswordException>()
            .And.Message.Should().Be("Password cannot be null or empty");
    }

    [Theory]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("123")]
    [InlineData("1234")]
    [InlineData("12345")]
    [InlineData("123456")]
    [InlineData("1234567")]
    public void ShouldFailIfPasswordLenIsLessThanMinimumChars(string plainText)
    {
        // Act
        Action action = () => Password.ShouldCreate(plainText!);

        // Assert
        action.Should().ThrowExactly<InvalidPasswordException>()
            .And.Message.Should().Contain("Password should have at least");
    }

    [Fact]
    public void ShouldFailIfPasswordLenIsGreaterThanMaxChars()
    {
        // Arrange
        string plainText = new string('a', Constaint.Password.MaxLength);

        // Act
        Action action = () => Password.ShouldCreate(plainText!);

        // Assert
        action.Should().ThrowExactly<InvalidPasswordException>()
            .And.Message.Should().Contain("Password should have less than");
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("123456789")]
    [InlineData("1234567890")]
    [InlineData("1234567890!")]
    [InlineData("AaA4567890@")]
    [InlineData("balta#123")]
    public void ShouldHashPassword(string? plainText)
    {
        // Arrange & Act
        var password = Password.ShouldCreate(plainText!);

        // Assert
        password.Hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ShouldMatch_WhenPasswordIsCorrect_ReturnsTrue()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";
        var passwordObject = Password.ShouldCreate(plainTextPassword);
        var hash = passwordObject.Hash;

        // Act
        var isMatch = Password.ShouldMatch(hash, plainTextPassword);

        // Assert
        isMatch.Should().BeTrue();
    }

    [Fact]
    public void ShouldMatch_WhenPasswordIsIncorrect_ReturnsFalse()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";
        var passwordObject = Password.ShouldCreate(plainTextPassword);
        var hash = passwordObject.Hash;
        var wrongPassword = "WrongPassword";

        // Act
        var isMatch = Password.ShouldMatch(hash, wrongPassword);

        // Assert
        isMatch.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(PasswordTheoryData.SpecialCharsPositions), MemberType = typeof(PasswordTheoryData))]
    public void ShouldGenerateStrongPassword_WithSpecialChars(int position)
    {
        // Arrange
        bool includeSpecialChars = true;
        bool upperCase = false;
        int specialCharsPosition = Constaint.Password.ValidChars.Length + position;
        char specialChar = Constaint.Password.CharSet[specialCharsPosition];

        _randomProviderMock.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(specialCharsPosition);

        // Act
        var password_generated = Password.ShouldGenerate(
            _randomProviderMock.Object,
            includeSpecialChars: includeSpecialChars,
            upperCase: upperCase);

        // Assert
        password_generated.Should().Contain(specialChar.ToString());
    }

    [Fact]
    public void ShouldGenerateStrongPassword()
    {
        // Arrange
        bool includeSpecialChars = true;
        bool upperCase = true;
        char firstCharUpper = 'A';
        char firstSpecialChar = '!';
        int positionUpper = 26;
        int specialCharsPosition = Constaint.Password.ValidChars.Length;

        _randomProviderMock.SetupSequence(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(positionUpper)
            .Returns(specialCharsPosition);

        // Act
        var password_generated = Password.ShouldGenerate(
           _randomProviderMock.Object,
           includeSpecialChars: includeSpecialChars,
           upperCase: upperCase);

        // Assert
        password_generated.Should().Contain(firstCharUpper.ToString());
        password_generated.Should().Contain(firstSpecialChar.ToString());
    }

    [Fact]
    public void ShouldImplicitConvertToString()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";

        // Act
        Password password = plainTextPassword;

        // Assert
        password.Hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void ShouldReturnHashAsStringWhenCallToStringMethod()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";

        // Act
        var password = Password.ShouldCreate(plainTextPassword);

        // Assert
        password.ToString().Should().Be(password.Hash);
    }

    [Fact]
    public void ShouldMarkPasswordAsExpired()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";
        var password = Password.ShouldCreate(plainTextPassword);
        DateTime dateTime = new DateTime(2024, 10, 1, 12, 0, 0);
        var dateTimeProvider = new DateTimeProviderFake(dateTime);

        // Act
        password.MarkAsExpired(dateTimeProvider);

        // Assert
        password.ExpiresAtUtc.Should().Be(dateTime);
    }

    [Fact]
    public void ShouldFailIfPasswordIsExpired() => Assert.Fail();

    [Fact]
    public void ShouldMarkPasswordAsMustChange()
    {
        // Arrange
        var plainTextPassword = "SecurePassword123!";
        var password = Password.ShouldCreate(plainTextPassword);

        // Act
        password.MarkAsMustChange();

        // Assert
        password.MustChange.Should().BeTrue();
    }

    [Fact]
    public void ShouldFailIfPasswordIsMarkedAsMustChange() => Assert.Fail();


}