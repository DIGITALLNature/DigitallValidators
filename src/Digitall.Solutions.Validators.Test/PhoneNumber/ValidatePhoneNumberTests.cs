using System;
using AwesomeAssertions;
using Digitall.APower;
using Digitall.Solutions.Validators.Model.Dataverse;
using Digitall.Solutions.Validators.PhoneNumber;
using Digitall.Testing;
using Digitall.Testing.Extensions;
using Microsoft.Xrm.Sdk;

namespace Digitall.Solutions.Validators.Test.PhoneNumber;

public class ValidatePhoneNumberTests
{
  [Test]
  public void ShouldReturnErrorWhenRegionIsNotProvidedAndPhoneNumberIsNotInternationalFormat()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "017728371992")
      .BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeFalse();
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should()
      .Be(PhoneNumberErrorCodes.InvalidCountryCode);
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should()
      .NotBeNullOrWhiteSpace();
  }

  [Test]
  public void ShouldReturnErrorWhenNoPhoneNumberIsProvided()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "QWERTZ").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeFalse();
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should()
      .Be(PhoneNumberErrorCodes.NotANumber);
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should()
      .NotBeNullOrWhiteSpace();
  }

  [Test]
  public void ShouldReturnErrorWhenPhoneNumberDoesNotMatchRegion()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "+491778261829")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "US").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeFalse();
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should()
      .Be(PhoneNumberErrorCodes.RegionMismatch);
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should()
      .NotBeNullOrWhiteSpace();
  }

  [Test]
  public void ShouldReturnErrorWhenPhoneNumberIsInvalid()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "+49123").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeFalse();
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should()
      .Be(PhoneNumberErrorCodes.InvalidPhoneNumber);
    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should()
      .NotBeNullOrWhiteSpace();
  }

  [Test]
  public void ShouldNotReturnErrorWhenPhoneNumberIsValid()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "DE").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeTrue();
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
  }

  [Test]
  public void ShouldReturnFormattedPhoneNumberWhenFormatIsProvided()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "DE")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Format, "INTERNATIONAL").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeTrue();
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters.Should()
      .ContainKey(DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber].As<string>()
      .Should().Be("+49 177 5628291");
  }

  [Test]
  public void ShouldReturnFormattedPhoneNumberAsE164WhenNoFormatIsProvided()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "DE").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    sut.Execute(serviceProvider);

    // Assert
    var executionContext = serviceProvider.GetExecutionContext();

    executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should()
      .BeTrue();
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
    executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    executionContext.OutputParameters.Should()
      .ContainKey(DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber);
    executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber].As<string>()
      .Should().Be("+491775628291");
  }

  [Test]
  public void ShouldThrowWhenInvalidCountryCodeIsProvided()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "LOL").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    Action action = () => sut.Execute(serviceProvider);

    // Assert
    action.Should().Throw<InvalidPluginExecutionException>();
  }

  [Test]
  public void ShouldThrowWhenInvalidFormatIsProvided()
  {
    // Arrange
    var serviceProvider = new PluginExecutionContextBuilder()
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, "DE")
      .WithInputParameter(DgtValidatePhoneNumberRequest.InParameters.Format, "E123").BuildServiceProvider();

    var sut = new ValidatePhoneNumber();

    // Act
    Action action = () => sut.Execute(serviceProvider);

    // Assert
    action.Should().Throw<InvalidPluginExecutionException>();
  }
}