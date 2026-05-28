namespace OrbitalGuardian.Repositories
{
    public interface IArquivoRepository
    {
        void SalvarLinha(string caminhoArquivo, string conteudo);
        void SalvarTexto(string caminhoArquivo, string conteudo);
        List<string> LerLinhas(string caminhoArquivo);
        void LimparArquivo(string caminhoArquivo);
    }
}