using System;
using dgt.registration;
using Digitall.APower;
using Digitall.Solutions.Validators.Model.Dataverse;
using IbanNet;
using IbanNet.Registry;
using IbanNet.Validation.Rules;

namespace Digitall.Solutions.Validators.Iban
{
  [CustomApiRegistration(SdkMessageNames.DgtValidateIban)]
  public class ValidateIban : PluginSkeleton
  {
    protected override void ExecuteInternal(IServiceProvider serviceProvider)
    {
      var executionContext = serviceProvider.GetExecutionContext();

      executionContext.GetInputParameter(DgtValidateIbanRequest.InParameters.Iban, out string inputValue);
      executionContext.GetInputParameter(DgtValidateIbanRequest.InParameters.AllowedCountries,
        out string[] allowedCountries);
      executionContext.GetInputParameter(DgtValidateIbanRequest.InParameters.RejectedCountries,
        out string[] rejectedCountries);

      var validatorOptions = new IbanValidatorOptions { Registry = IbanRegistry.Default };
      if (allowedCountries != null && allowedCountries.Length > 0)
        validatorOptions.Rules.Add(new AcceptCountryRule(allowedCountries));
      if (rejectedCountries != null && rejectedCountries.Length > 0)
        validatorOptions.Rules.Add(new RejectCountryRule(rejectedCountries));

      var parser = new IbanParser(new IbanValidator(validatorOptions));
      try
      {
        var iban = parser.Parse(inputValue);

        var electronicFormat = iban.ToString(IbanFormat.Electronic);
        var printFormat = iban.ToString(IbanFormat.Print);

        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.IsValid, true);
        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.ElectronicFormat, electronicFormat);
        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.PrintFormat, printFormat);
      }
      catch (IbanFormatException e)
      {
        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.IsValid, false);
        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.ErrorCode,
          e.Result?.Error?.GetType().Name);
        executionContext.SetOutputParameter(DgtValidateIbanResponse.OutParameters.Message, e.Message);
      }
    }
  }
}