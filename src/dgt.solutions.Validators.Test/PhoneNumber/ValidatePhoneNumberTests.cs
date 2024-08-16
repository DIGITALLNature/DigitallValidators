using System;
using dgt.solutions.Validators.PhoneNumber;
using Digitall.APower.Model;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Plugins;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace dgt.solutions.Validators.Test.PhoneNumber
{
  [TestClass]
  public class ValidatePhoneNumberTests
  {
    [TestMethod]
    public void ShouldReturnErrorWhenRegionIsNotProvidedAndPhoneNumberIsNotInternationalFormat()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "017728371992");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeFalse();
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should().Be(PhoneNumberErrorCodes.InvalidCountryCode);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public void ShouldReturnErrorWhenNoPhoneNumberIsProvided()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "QWERTZ");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeFalse();
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should().Be(PhoneNumberErrorCodes.NotANumber);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public void ShouldReturnErrorWhenPhoneNumberDoesNotMatchRegion()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "+491778261829");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "US");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeFalse();
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should().Be(PhoneNumberErrorCodes.RegionMismatch);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should().NotBeNullOrWhiteSpace();
    }
    
    [TestMethod]
    public void ShouldReturnErrorWhenPhoneNumberIsInvalid()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "+49123");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeFalse();
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.ErrorCode].As<string>().Should().Be(PhoneNumberErrorCodes.InvalidPhoneNumber);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.Message].As<string>().Should().NotBeNullOrWhiteSpace();
    }
    
    [TestMethod]
    public void ShouldNotReturnErrorWhenPhoneNumberIsValid()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "DE");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeTrue();
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
    }
    
    [TestMethod]
    public void ShouldReturnFormattedPhoneNumberWhenFormatIsProvided()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "DE");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Format, "INTERNATIONAL");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeTrue();
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber].As<string>().Should().Be("+49 177 5628291");
    }
    
    [TestMethod]
    public void ShouldReturnFormattedPhoneNumberAsE164WhenNoFormatIsProvided()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "DE");

      // Act
      context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.IsValid);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.IsValid].As<bool>().Should().BeTrue();
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode);
      executionContext.OutputParameters.Should().NotContainKey(DgtValidatePhoneNumberResponse.OutParameters.Message);
      executionContext.OutputParameters.Should().ContainKey(DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber);
      executionContext.OutputParameters[DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber].As<string>().Should().Be("+491775628291");
    }
    
    [TestMethod]
    public void ShouldThrowWhenInvalidCountryCodeIsProvided()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "LOL");

      // Act
      Action action = () => context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      action.Should().Throw<InvalidPluginExecutionException>();
    }
    
    [TestMethod]
    public void ShouldThrowWhenInvalidFormatIsProvided()
    {
      // Arrange
      var context = MiddlewareBuilder.New().AddCrud().AddFakeMessageExecutors().UseCrud().UseMessages()
        .SetLicense(FakeXrmEasyLicense.RPL_1_5).Build();

      var executionContext = context.GetDefaultPluginContext();
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber, "01775628291");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Region, "DE");
      executionContext.InputParameters.Add(DgtValidatePhoneNumberRequest.InParameters.Format, "E123");

      // Act
      Action action = () => context.ExecutePluginWith<ValidatePhoneNumber>(executionContext);

      // Assert
      action.Should().Throw<InvalidPluginExecutionException>();
    }
  }
}