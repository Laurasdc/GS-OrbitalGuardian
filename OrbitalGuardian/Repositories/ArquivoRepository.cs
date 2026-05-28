namespace OrbitalGuardian.Repositories
{
    public class ArquivoRepository : IArquivoRepository
    {
        public void SalvarLinha(string caminhoArquivo, string conteudo)
        {
            try
            {
                string? diretorio = Path.GetDirectoryName(caminhoArquivo);

                if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }

                using StreamWriter writer = new StreamWriter(caminhoArquivo, append: true);
                writer.WriteLine(conteudo);
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException("Sem permissão para gravar no arquivo de histórico.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new IOException("Diretório do arquivo de histórico não encontrado.");
            }
            catch (IOException ex)
            {
                throw new IOException($"Erro ao salvar arquivo: {ex.Message}");
            }
        }

        public List<string> LerLinhas(string caminhoArquivo)
        {
            try
            {
                if (!File.Exists(caminhoArquivo))
                {
                    return new List<string>();
                }

                return File.ReadAllLines(caminhoArquivo).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException("Sem permissão para ler o arquivo de histórico.");
            }
            catch (IOException ex)
            {
                throw new IOException($"Erro ao ler arquivo: {ex.Message}");
            }
        }
    }
}