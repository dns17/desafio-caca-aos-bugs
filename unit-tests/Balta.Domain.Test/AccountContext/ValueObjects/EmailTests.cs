using Balta.Domain.AccountContext.ValueObjects;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.SharedContext.Extensions;
using Balta.Domain.Test.TestUtils;
using Balta.Domain.Test.TestUtils.Constaints;
using FluentAssertions;

namespace Balta.Domain.Test.AccountContext.ValueObjects;

public class EmailTests
{
    private readonly DateTimeProviderFake _dateTimeProviderFake = new();

    [Fact]
    public void ShouldLowerCaseEmail()
    {
        // Arrange
        string address = "TESTE@BALTA.IO";

        // Act
        var email = Email.ShouldCreate(address, _dateTimeProviderFake);

        // Assert
        email.Address.Should().Be(address.ToLower());
    }

    [Theory]
    [InlineData("  teste@balta.io")]
    [InlineData("  teste@balta.io  ")]
    [InlineData("teste@balta.io  ")]
    public void ShouldTrimEmail(string invalid_email)
    {
        // Arrange
        string address = Constaint.Email.Address;

        //Act
        var email = Email.ShouldCreate(invalid_email, _dateTimeProviderFake);

        // Assert
        email.Address.Should().Be(address);
    }

    [Fact]
    public void ShouldFailIfEmailIsNull()
    {
        // Arrange
        string address = null!;

        // Act
        var action = () => Email.ShouldCreate(address, _dateTimeProviderFake);

        // Assert
        action.Should().Throw<InvalidEmailException>();
    }

    [Fact]
    public void ShouldFailIfEmailIsEmpty()
    {
        // Arrange
        string address = "";

        // Act
        Action action = () => Email.ShouldCreate(address, _dateTimeProviderFake);

        // Assert
        action.Should().Throw<InvalidEmailException>();
    }

    [Theory]
    [InlineData("teste")]
    [InlineData("teste@balta")]
    [InlineData("testebalta.com")]
    public void ShouldFailIfEmailIsInvalid(string invalid_email)
    {
        // Act
        var action = () => Email.ShouldCreate(invalid_email, _dateTimeProviderFake);

        // Assert
        action.Should().Throw<InvalidEmailException>();
    }

    [Theory]
    [InlineData("teste@balta.io")]
    [InlineData("teste@balta.com.br")]
    [InlineData("teste@balta.com.gov")]
    public void ShouldPassIfEmailIsValid(string valid_email)
    {
        // Act
        var email = Email.ShouldCreate(valid_email, _dateTimeProviderFake);

        // Assert

        email.Should().NotBeNull().And.BeOfType<Email>();
        email.Address.Should().Be(valid_email);
    }

    [Fact]
    public void ShouldHashEmailAddress()
    {
        // Arrange
        string address = Constaint.Email.Address;

        // Act
        var email = Email.ShouldCreate(address, _dateTimeProviderFake);

        // Assert
        email.Hash.Should().Be(address.ToBase64());
    }

    [Fact]
    public void ShouldExplicitConvertFromString()
    {
        // Arrange
        string address = Constaint.Email.Address;

        // Act & Assert
        Email email = address;

        // Assert
        email.Address.Should().Be(address);
    }

    [Fact]
    public void ShouldExplicitConvertToString()
    {
        // Arrange
        Email email = Email.ShouldCreate(
            Constaint.Email.Address,
            _dateTimeProviderFake);

        // Act & Assert
        string address = email;
    }

    [Fact]
    public void ShouldReturnEmailWhenCallToStringMethod()
    {
        // Arrange
        Email email = Email.ShouldCreate(
            Constaint.Email.Address,
            _dateTimeProviderFake);

        // Act
        string address = email.ToString();

        // Assert
        address.Should().Be(Constaint.Email.Address);
    }
}