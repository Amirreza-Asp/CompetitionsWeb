namespace Competitions.Web.Utilities
{
    public static class StringHelpers
    {

        public static bool IsPDF(this String fileName)
        {
            return Path.GetExtension(fileName) == ".pdf";
        }

    }
}
