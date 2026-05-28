namespace OrbitalGuardian.Repositories
{
    public interface IArquivoRepository
    {
        void SalvarLinha(string caminhoArquivo, string conteudo);
        List<string> LerLinhas(string caminhoArquivo);
    }
}