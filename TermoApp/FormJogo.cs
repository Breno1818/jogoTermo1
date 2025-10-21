using System;
using System.Windows.Forms;
using System.Drawing;
using TermoLib;

namespace TermoApp
{
    public partial class FormJogo : Form
    {
        public Termo termo;
        int coluna = 1;

        public FormJogo()
        {
            InitializeComponent();
            termo = new Termo();
            this.KeyPreview = true;
            this.KeyDown += FormJogo_KeyDown;

            foreach (Control c in Controls)
                if (c is Button btn)
                    btn.TabStop = false;

            this.ActiveControl = null;
            this.Focus();
        }

        private void FormJogo_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.KeyCode.ToString().ToUpper();

            if (e.KeyCode == Keys.Enter)
            {
                btnEnter_Click(null, null);
                return;
            }

            if (e.KeyCode == Keys.Back)
            {
                btnBackspace_Click(null, null);
                return;
            }

            if (key.Length == 1 && key[0] >= 'A' && key[0] <= 'Z')
            {
                Control[] botoes = this.Controls.Find("btn" + key, true);
                if (botoes.Length > 0 && botoes[0] is Button botao)
                {
                    btnTeclado_Click(botao, EventArgs.Empty);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnEnter_Click(null, null);
                return true;
            }
            if (keyData == Keys.Back)
            {
                btnBackspace_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnTeclado_Click(object sender, EventArgs e)
        {
            if (coluna > 5) return;
            var button = (Button)sender;
            var linha = termo.palavraAtual;
            var nomeBotao = $"btn{linha}{coluna}";
            var buttonTabuleiro = RetornaBotao(nomeBotao);
            buttonTabuleiro.Text = button.Text;
            coluna++;
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            var palavra = "";
            for (int i = 1; i <= 5; i++)
            {
                var nomeBotao = $"btn{termo.palavraAtual}{i}";
                var botao = RetornaBotao(nomeBotao);
                palavra += botao.Text;
            }

            try
            {
                termo.ChecaPalavra(palavra);
                AtualizaTabuleiro();
                coluna = 1;
                this.ActiveControl = null;
                this.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (coluna > 1)
            {
                var nomeBotao = $"btn{termo.palavraAtual}{coluna - 1}";
                var botao = RetornaBotao(nomeBotao);
                botao.Text = "";
                coluna--;
            }
        }

        private Button RetornaBotao(string name)
        {
            return (Button)Controls.Find(name, true)[0];
        }

        private void AtualizaTabuleiro()
        {
            for (int col = 1; col <= 5; col++)
            {
                var letra = termo.tabuleiro[termo.palavraAtual - 2][col - 1];
                var nomeBotaoTab = $"btn{termo.palavraAtual - 1}{col}";
                var botaoTab = RetornaBotao(nomeBotaoTab);
                var nomeBotaoKey = $"btn{letra.Caracter}";
                var botaoKey = RetornaBotao(nomeBotaoKey);

                if (letra.Cor == 'A')
                {
                    botaoTab.BackColor = Color.Yellow;
                    botaoKey.BackColor = Color.Yellow;
                }
                else if (letra.Cor == 'V')
                {
                    botaoTab.BackColor = Color.Green;
                    botaoKey.BackColor = Color.Green;
                }
                else
                {
                    botaoTab.BackColor = Color.Gray;
                    botaoKey.BackColor = Color.Gray;
                }
            }
        }
    }
}
