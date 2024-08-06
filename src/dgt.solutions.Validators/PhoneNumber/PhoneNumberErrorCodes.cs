namespace dgt.solutions.Validators.PhoneNumber
{
  public static class PhoneNumberErrorCodes
  {
    // PhoneNumbers.ErrorType
    public const string InvalidCountryCode = "INVALID_COUNTRY_CODE";
    public const string NotANumber = "NOT_A_NUMBER";
    public const string TooShortAfterIdd = "TOO_SHORT_AFTER_IDD";
    public const string TooShortNsn = "TOO_SHORT_NSN";
    public const string TooLong = "TOO_LONG";
    
    // custom error codes
    public const string RegionMismatch = "REGION_MISMATCH";
    public const string InvalidPhoneNumber = "INVALID_PHONE_NUMBER";
  }
}