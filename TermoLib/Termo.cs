using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TermoLib
{
    public class Letra
    {
        public char Caracter;
        public char Cor;
        public Letra(char caracter, char cor)
        {
            Caracter = caracter;
            Cor = cor;
        }
    }

    public class Termo
    {
        public List<string> palavras;
        public string palavraSorteada;
        public List<List<Letra>> tabuleiro;
        public Dictionary<char, char> teclado;
        public int palavraAtual;

        public Termo()
        {
            CarregaPalavras("Palavras.txt");
            SorteiaPalavra();
            palavraAtual = 1;
            tabuleiro = new List<List<Letra>>();
            teclado = new Dictionary<char, char>();
            for (int i = 65; i <= 90; i++)
            {
                teclado.Add((char)i, 'C');
            }
        }

        public void CarregaPalavras(string fileName)
        {
            palavras = File.ReadAllLines(fileName)
                            .Select(p => p.Trim().ToUpper())
                            .Where(p => p.Length == 5)
                            .ToList();
        }

        public async Task CarregaPalavrasOnlineAsync(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string conteudoOnline = await client.GetStringAsync(url);
                    var linhas = conteudoOnline.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    palavras = linhas
                                .Select(p => p.Trim().ToUpper())
                                .Where(p => p.Length == 5)
                                .ToList();

                    SorteiaPalavra();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível carregar palavras da internet. Verifique a conexão. Erro: " + ex.Message);
            }
        }

        public void SorteiaPalavra()
        {
            if (palavras == null || palavras.Count == 0)
            {
                palavraSorteada = "ERRO!";
                return;
            }
            Random rdn = new Random();
            var index = rdn.Next(0, palavras.Count());
            palavraSorteada = palavras[index];
        }

        public void ChecaPalavra(string palavra)
        {
            palavra = palavra.Trim().ToUpper();

            if (palavra.Length != 5)
            {
                throw new Exception("Tamanho incorreto! A palavra deve ter exatamente 5 letras.");
            }
            if (!palavras.Contains(palavra))
            {
                throw new Exception("Palavra inválida! Digite uma palavra existente.");
            }
            var palavraTabuleiro = new List<Letra>();
            char[] letrasRestantes = palavraSorteada.ToCharArray();
            for (int i = 0; i < palavra.Length; i++)
            {
                if (palavra[i] == palavraSorteada[i])
                {
                    palavraTabuleiro.Add(new Letra(palavra[i], 'V'));
                    letrasRestantes[i] = '*';
                }
                else
                {
                    palavraTabuleiro.Add(new Letra(palavra[i], ' '));
                }
            }
            for (int i = 0; i < palavra.Length; i++)
            {
                if (palavraTabuleiro[i].Cor == 'V')
                {
                    continue;
                }
                char letra = palavra[i];
                int index = Array.IndexOf(letrasRestantes, letra);

                if (index != -1)
                {
                    palavraTabuleiro[i].Cor = 'A';
                    letrasRestantes[index] = '*';
                }
                else
                {
                    palavraTabuleiro[i].Cor = 'P';
                }
            }
            tabuleiro.Add(palavraTabuleiro);
            foreach (var letraNoTabuleiro in palavraTabuleiro)
            {
                char letra = letraNoTabuleiro.Caracter;
                char corNova = letraNoTabuleiro.Cor;
                char corAntiga = teclado[letra];
                if (corAntiga == 'V') continue;
                if (corNova == 'V') { teclado[letra] = 'V'; continue; }
                if (corAntiga == 'A') continue;
                if (corNova == 'A') { teclado[letra] = 'A'; continue; }
                teclado[letra] = corNova;
            }
            palavraAtual++;
        }
    }
}