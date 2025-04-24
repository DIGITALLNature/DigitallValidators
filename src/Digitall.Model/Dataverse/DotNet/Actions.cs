using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

// ReSharper disable All
namespace Digitall.APower.Model
{
	[RequestProxy("dgt_ValidatePhoneNumber")]
	public class DgtValidatePhoneNumberRequest : OrganizationRequest
	{
		public DgtValidatePhoneNumberRequest()
		{
			RequestName = "dgt_ValidatePhoneNumber";
		}

		#region consts
		public const string RequestLogicalName = "dgt_ValidatePhoneNumber";
		#endregion

		#region InParameters
		public struct InParameters
		{
			public const string Format = "Format";
			public const string PhoneNumber = "PhoneNumber";
			public const string Region = "Region";
		}
		#endregion

		public string Format
		{
			get
			{
				if(base.Parameters.Contains("Format"))
				{
					return (string)base.Parameters["Format"];
				}
				return default(string);
			}
			set
			{
				base.Parameters["Format"] = value;
			}
		}

		public string PhoneNumber
		{
			get
			{
				if(base.Parameters.Contains("PhoneNumber"))
				{
					return (string)base.Parameters["PhoneNumber"];
				}
				return default(string);
			}
			set
			{
				base.Parameters["PhoneNumber"] = value;
			}
		}

		public string Region
		{
			get
			{
				if(base.Parameters.Contains("Region"))
				{
					return (string)base.Parameters["Region"];
				}
				return default(string);
			}
			set
			{
				base.Parameters["Region"] = value;
			}
		}

	}

	[ResponseProxy("dgt_ValidatePhoneNumber")]
	public class DgtValidatePhoneNumberResponse : OrganizationResponse
	{
		#region OutParameters
		public struct OutParameters
		{
			public const string ErrorCode = "ErrorCode";
			public const string FormattedPhoneNumber = "FormattedPhoneNumber";
			public const string IsValid = "IsValid";
			public const string Message = "Message";
		}
		#endregion

		public string ErrorCode
		{
			get
			{
				if(base.Results.Contains("ErrorCode"))
				{
					return (string)base.Results["ErrorCode"];
				}
				return default(string);
			}
		}

		public string FormattedPhoneNumber
		{
			get
			{
				if(base.Results.Contains("FormattedPhoneNumber"))
				{
					return (string)base.Results["FormattedPhoneNumber"];
				}
				return default(string);
			}
		}

		public bool IsValid
		{
			get
			{
				if(base.Results.Contains("IsValid"))
				{
					return (bool)base.Results["IsValid"];
				}
				return default(bool);
			}
		}

		public string Message
		{
			get
			{
				if(base.Results.Contains("Message"))
				{
					return (string)base.Results["Message"];
				}
				return default(string);
			}
		}

	}

}
