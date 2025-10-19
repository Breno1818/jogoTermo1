using Microsoft.VisualStudio.TestTools.UnitTesting;
using TermoLib;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TermoTest
{
    [TestClass]
    public sealed class TestTermo
    {

        [TestMethod]
        public async Task TestCarregarPalavrasOnline()
        {
            Termo termo = new Termo();

            // <-- URL pega no github, criador: Casdak7 -->
            string url = "https://raw.githubusercontent.com/Casdak7/palavras/refs/heads/master/palavras.txt";

            // Act (Ação)
            await termo.CarregaPalavrasOnlineAsync(url);

            // Assert (Verificação)
            // 1. Verifica se a lista de palavras não está vazia.
            Assert.IsTrue(termo.palavras.Count > 0, "A lista de palavras carregada da internet não deveria estar vazia.");

            // 2. Verifica se TODAS as palavras na lista têm 5 letras.
            Assert.IsTrue(termo.palavras.All(p => p.Length == 5), "Nem todas as palavras carregadas têm 5 letras.");

            // 3. Imprime uma confirmação no console.
            Console.WriteLine($"✅ Sucesso! Carregadas {termo.palavras.Count} palavras de 5 letras da URL.");
            Console.WriteLine("Nova palavra sorteada: " + termo.palavraSorteada);
        }

        [TestMethod]
        public void TestJogo()
        {
            // Forçar uma palavra conhecida para tornar o teste previsível
            Termo termo = new Termo();
            termo.palavraSorteada = "TIGRE";

            // (O resto do seu TestJogo e outros testes continuam aqui...)
        }

        // ... (Resto dos seus testes e o método ImprimirJogo)
    }
}