namespace OrbitalGuardian.Utils
{
    public static class FormatadorData
    {
        public static string Formatar(DateTime data)
        {
            return data.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}