using System;
using System.Globalization;
using dgt.registration;
using Digitall.APower;
using Digitall.APower.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using PhoneNumbers;

namespace dgt.solutions.Validators.PhoneNumber
{
  [CustomApiRegistration(SdkMessageNames.DgtValidatePhoneNumber)]
  public class ValidatePhoneNumber : IPlugin
  {
    public void Execute(IServiceProvider serviceProvider)
    {
      var executionContext = serviceProvider.Get<IPluginExecutionContext>(); // serviceProvider.GetExecutionContext();
      var inputParameters = ParseInputParameters(executionContext);

      var phoneNumberUtil = PhoneNumberUtil.GetInstance();

      // parse
      PhoneNumbers.PhoneNumber phoneNumber;
      try
      {
        phoneNumber = phoneNumberUtil.Parse(inputParameters.PhoneNumber, inputParameters.Region);
      }
      catch (NumberParseException e)
      {
        executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.IsValid, false);
        executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode,
          e.ErrorType.ToString());
        executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.Message, e.Message);
        return;
      }

      // validate
      var isValid = string.IsNullOrWhiteSpace(inputParameters.Region)
        ? ValidateWithoutRegion(phoneNumberUtil, phoneNumber, executionContext)
        : ValidateWithRegion(phoneNumberUtil, phoneNumber, executionContext, inputParameters.Region);
      if (!isValid) return;

      // format
      var formattedPhoneNumber = phoneNumberUtil.Format(phoneNumber, inputParameters.Format);
      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.FormattedPhoneNumber,
        formattedPhoneNumber);
    }

    private static bool ValidateWithRegion(PhoneNumberUtil phoneNumberUtil, PhoneNumbers.PhoneNumber phoneNumber,
      IPluginExecutionContext executionContext, string region)
    {
      var isValid = phoneNumberUtil.IsValidNumberForRegion(phoneNumber, region);
      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.IsValid, isValid);
      if (isValid) return true;

      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode,
        PhoneNumberErrorCodes.RegionMismatch);
      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.Message,
        $"Phone number is not valid for region '{region}'.");

      return false;
    }

    private static bool ValidateWithoutRegion(PhoneNumberUtil phoneNumberUtil, PhoneNumbers.PhoneNumber phoneNumber,
      IPluginExecutionContext executionContext)
    {
      var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);
      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.IsValid, isValid);
      if (isValid) return true;

      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.ErrorCode,
        PhoneNumberErrorCodes.InvalidPhoneNumber);
      executionContext.SetOutputParameter(DgtValidatePhoneNumberResponse.OutParameters.Message,
        "Phone number is not valid.");

      return false;
    }

    private static PhoneNumberValidationParameters ParseInputParameters(IPluginExecutionContext executionContext)
    {
      executionContext.GetInputParameter(DgtValidatePhoneNumberRequest.InParameters.PhoneNumber,
        out string phoneNumber);
      executionContext.GetInputParameter(DgtValidatePhoneNumberRequest.InParameters.Region, out string region);
      executionContext.GetInputParameter(DgtValidatePhoneNumberRequest.InParameters.Format, out string format);

      var regionCode = ParseRegionCode(region);
      var phoneNumberFormat = ParsePhoneNumberFormat(format);

      return new PhoneNumberValidationParameters
      {
        PhoneNumber = phoneNumber, Region = regionCode, Format = phoneNumberFormat
      };
    }

    private static string ParseRegionCode(string region)
    {
      if (string.IsNullOrWhiteSpace(region)) return null;

      try
      {
        var regionInfo = new RegionInfo(region);
        return regionInfo.TwoLetterISORegionName;
      }
      catch (ArgumentException)
      {
        throw new InvalidPluginExecutionException(OperationStatus.Succeeded,
          $"'{region}' is not a valid ISO 3166 two-letter country code.");
      }
    }

    private static PhoneNumberFormat ParsePhoneNumberFormat(string format)
    {
      if (string.IsNullOrWhiteSpace(format)) return PhoneNumberFormat.E164;

      if (!Enum.TryParse(format.ToUpperInvariant(), out PhoneNumberFormat phoneNumberFormat))
        throw new InvalidPluginExecutionException(OperationStatus.Succeeded,
          $"Format '{format}' is not valid. Possible values are: {string.Join(",", Enum.GetNames(typeof(PhoneNumberFormat)))}");

      return phoneNumberFormat;
    }

    private class PhoneNumberValidationParameters
    {
      public string PhoneNumber { get; set; }
      public string Region { get; set; }
      public PhoneNumberFormat Format { get; set; }
    }
  }
}