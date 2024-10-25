using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.Test.TestUtils.Constaints;
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
    public void ShouldVerifyPasswordHash() => Assert.Fail();

    [Fact]
    public void ShouldGenerateStrongPassword() => Assert.Fail();

    [Fact]
    public void ShouldImplicitConvertToString() => Assert.Fail();

    [Fact]
    public void ShouldReturnHashAsStringWhenCallToStringMethod() => Assert.Fail();

    [Fact]
    public void ShouldMarkPasswordAsExpired() => Assert.Fail();

    [Fact]
    public void ShouldFailIfPasswordIsExpired() => Assert.Fail();

    [Fact]
    public void ShouldMarkPasswordAsMustChange() => Assert.Fail();

    [Fact]
    public void ShouldFailIfPasswordIsMarkedAsMustChange() => Assert.Fail();

    
}