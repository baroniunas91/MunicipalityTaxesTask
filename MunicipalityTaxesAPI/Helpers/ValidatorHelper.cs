namespace MunicipalityTaxesAPI.Helpers
{
    public static class ValidatorHelper
    {
        public static bool ValidDate(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return false;
            }

            return dateTime >= new DateTime(1970, 1, 1) && dateTime < new DateTime(9999, 1, 1);
        }
    }
}