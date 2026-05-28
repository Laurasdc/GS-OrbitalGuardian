using OrbitalGuardian.Application;

namespace OrbitalGuardian
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SistemaOrbitalGuardian sistema = new SistemaOrbitalGuardian();
            sistema.Executar();
        }
    }
}