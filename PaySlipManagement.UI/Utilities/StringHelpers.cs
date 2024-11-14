namespace PaySlipManagement.UI.Utilities
{
    public static class StringHelpers
    {
        public static string MaskPanNumber(string PAN_Number)
        {
            if (string.IsNullOrEmpty(PAN_Number) || PAN_Number.Length < 10)
            {
                return PAN_Number;
            }
            return "xxxxxx" + PAN_Number.Substring(6);
        }
        // Mask PF Account Number
        public static string MaskPfAccountNumber(string pfAccountNumber)
        {
            if (string.IsNullOrEmpty(pfAccountNumber) || pfAccountNumber.Length < 12)
            {
                return pfAccountNumber;
            }
            // Masking all but the last 4 characters
            return "xxxxxxxx" + pfAccountNumber.Substring(pfAccountNumber.Length - 4);
        }

        // Mask Bank Account Number
        public static string MaskBankAccountNumber(long bankAccountNumber)
        {
            var bankAccountString = bankAccountNumber.ToString();

            if (string.IsNullOrEmpty(bankAccountString) || bankAccountString.Length < 6)
            {
                return bankAccountString;
            }
            // Masking all but the last 4 characters
            return "xxxxxxxxxx" + bankAccountString.Substring(bankAccountString.Length - 4);
        }

        // Mask UAN Number
        public static string MaskUanNumber(long uanNumber)
        {
            var uanString = uanNumber.ToString();

            if (string.IsNullOrEmpty(uanString) || uanString.Length < 12)
            {
                return uanString;
            }
            // Masking all but the last 4 characters
            return "xxxxxxxxxxxx" + uanString.Substring(uanString.Length - 4);
        }

        //Mask Phone number
        public static string MaskPhoneNumber(long PhoneNumber)
        {
            var phoneNumberString = PhoneNumber.ToString();
            if (string.IsNullOrEmpty(phoneNumberString) || phoneNumberString.Length < 10)
            {
                return phoneNumberString;
            }
            return "xxxxxx" + phoneNumberString.Substring(6);
        }
    }
}
