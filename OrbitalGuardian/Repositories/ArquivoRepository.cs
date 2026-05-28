namespace OrbitalGuardian.Repositories
{
    public class ArquivoRepository : IArquivoRepository
    {
        public void SalvarLinha(string caminhoArquivo, string conteudo)
        {
            try
            {
                CriarDiretorioSeNaoExistir(caminhoArquivo);

                using StreamWriter writer = new StreamWriter(caminhoArquivo, append: true);
                writer.WriteLine(conteudo);
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException("Sem permissão para gravar no arquivo.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new IOException("Diretório do arquivo não encontrado.");
            }
            catch (IOException ex)
            {
                throw new IOException($"Erro ao salvar arquivo: {ex.Message}");
            }
        }

        public void SalvarTexto(string caminhoArquivo, string conteudo)
        {
            try
            {
                CriarDiretorioSeNaoExistir(caminhoArquivo);

                File.WriteAllText(caminhoArquivo, conteudo);
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException("Sem permissão para gravar no arquivo.");
            }
            catch (DirectoryNotFoundException)
            {
                throw new IOException("Diretório do arquivo não encontrado.");
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
                throw new IOException("Sem permissão para ler o arquivo.");
            }
            catch (IOException ex)
            {
                throw new IOException($"Erro ao ler arquivo: {ex.Message}");
            }
        }

        public void LimparArquivo(string caminhoArquivo)
        {
            try
            {
                CriarDiretorioSeNaoExistir(caminhoArquivo);

                File.WriteAllText(caminhoArquivo, string.Empty);
            }
            catch (UnauthorizedAccessException)
            {
                throw new IOException("Sem permissão para limpar o arquivo.");
            }
            catch (IOException ex)
            {
                throw new IOException($"Erro ao limpar arquivo: {ex.Message}");
            }
        }

        private void CriarDiretorioSeNaoExistir(string caminhoArquivo)
        {
            string? diretorio = Path.GetDirectoryName(caminhoArquivo);

            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
        }
    }
}