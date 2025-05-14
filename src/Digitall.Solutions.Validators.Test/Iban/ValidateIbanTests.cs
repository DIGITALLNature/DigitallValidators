using Digitall.APower;
using Digitall.Solutions.Validators.Iban;
using Digitall.Solutions.Validators.Model.Dataverse;
using Digitall.Testing;
using Digitall.Testing.Extensions;
using FluentAssertions;
using IbanNet.Validation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digitall.Solutions.Validators.Test.Iban
{
  [TestClass]
  public class ValidateIbanTests
  {
    [TestMethod]
    public void ShouldReturnFormattedIbanWhenValid()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE89370400440532013000").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, true);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ElectronicFormat,
        "DE89370400440532013000");
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.PrintFormat,
        "DE89 3704 0044 0532 0130 00");
    }

    [TestMethod]
    public void ShouldReturnErrorWhenCheckDigitsAreInvalid()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE89370400440532013012").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(InvalidCheckDigitsResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenLengthIsInvalid()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE8937040044053201300034")
        .BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(InvalidLengthResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenDoesNotMatchExpectedPattern()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE893704004405320130P2").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(InvalidStructureResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenCountryIsNotSupported()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "US89370400440532013000").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(UnknownCountryCodeResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenCountryCodeIsInvalid()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "89370400440532013000").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(IllegalCountryCodeCharactersResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenInvalidCharactersAreIncluded()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE370-4004405-32013000").BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(IllegalCharactersResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenCountryIsNotAllowed()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE75512108001245126199")
        .WithInputParameter(DgtValidateIbanRequest.InParameters.AllowedCountries, new[] { "FR" })
        .BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(CountryNotAcceptedResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }
    
    [TestMethod]
    public void ShouldReturnErrorWhenCountryIsRejected()
    {
      // Arrange
      var serviceProvider = new PluginExecutionContextBuilder()
        .WithInputParameter(DgtValidateIbanRequest.InParameters.Iban, "DE75512108001245126199")
        .WithInputParameter(DgtValidateIbanRequest.InParameters.RejectedCountries, new[] { "DE" })
        .BuildServiceProvider();

      var sut = new ValidateIban();

      // Act
      sut.Execute(serviceProvider);

      // Assert
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.IsValid, false);
      executionContext.OutputParameters.Should().Contain(DgtValidateIbanResponse.OutParameters.ErrorCode,
        nameof(CountryNotAcceptedResult));
      executionContext.OutputParameters.Should().ContainKey(DgtValidateIbanResponse.OutParameters.Message);
    }
  }
}