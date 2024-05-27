using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
    // 22593 & 23512
    public partial class FrmCaminhos : Form
    {
        SaveFileDialog Salvar;
        public FrmCaminhos()
        {
            InitializeComponent();
            Salvar = new SaveFileDialog();
        }

        ITabelaDeHash<Cidade> tabela;

        private void AtualizaCidades()
        {
            lsbListagem.Items.Clear();
            var cidades = tabela.Conteudo();
            foreach (Cidade cidade in cidades)
                lsbListagem.Items.Add(cidade);
        }

        private void btnLerArquivo_Click(object sender, EventArgs e)
        {
            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                if (rbBucketHash.Checked)
                    tabela = new BucketHash<Cidade>();
                else if (rbSondagemLinear.Checked)
                    tabela = new HashLinear<Cidade>();
                else if (rbSondagemQuadratica.Checked)
                    tabela = new HashQuadratico<Cidade>();
                else if (rbHashDuplo.Checked)
                    tabela = new HashDuplo<Cidade>();

                var txt = new StreamReader(dlgAbrir.FileName);
                while (!txt.EndOfStream)
                {
                    Cidade novaCidade = new Cidade();
                    novaCidade.LerRegistro(txt);
                    tabela.Inserir(novaCidade);
                }
                txt.Close();
                AtualizaCidades();
                DesenharPontos(pbMapa.CreateGraphics()); 
            }
        }

        private void DesenharPontos(Graphics g)
        {
            
            if (tabela != null)
            {
                var conteudo = tabela.Conteudo();
                Brush brush = Brushes.Black;
                int pontoSize = 5;

                double escalaX = pbMapa.Width; 
                double escalaY = pbMapa.Height; 

                foreach (var cidade in conteudo)
                {
                    int x = (int)(cidade.X * escalaX);
                    int y = (int)(cidade.Y * escalaY);

                    // desenha o ponto
                    g.FillEllipse(brush, x - pontoSize / 2, y - pontoSize / 2, pontoSize, pontoSize);

                    g.DrawString(cidade.NomeCidade, Font, brush, x + pontoSize, y);
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            Cidade novaCidade = new Cidade();
            novaCidade.NomeCidade = txtNome.Text;
            novaCidade.X = (double)udX.Value;
            novaCidade.Y = (double)udY.Value;

            if (tabela != null)
            {
                int posicao;
                if (!tabela.Existe(novaCidade, out posicao))
                {
                    tabela.Inserir(novaCidade);
                    AtualizaCidades();
                }
                else
                {
                    MessageBox.Show("A cidade já existe na tabela.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nomeCidadeBusca = txtNome.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeCidadeBusca))
            {
                MessageBox.Show("Por favor, insira o nome da cidade para realizar a busca.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tabela != null)
            {
                Cidade cidadeBusca = new Cidade { NomeCidade = nomeCidadeBusca };

                int posicao;
                bool encontrada = tabela.Existe(cidadeBusca, out posicao);

                if (encontrada)
                {
                    MessageBox.Show($"A cidade '{nomeCidadeBusca}' foi encontrada na posição {posicao} da tabela de hash.", "Resultado da Busca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"A cidade '{nomeCidadeBusca}' não foi encontrada na tabela de hash.", "Resultado da Busca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (lsbListagem.SelectedItem != null)
            {
                Cidade cidadeSelecionada = (Cidade)lsbListagem.SelectedItem;
                if (tabela != null)
                {
                    if (tabela.Remover(cidadeSelecionada))
                    {
                        AtualizaCidades();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao remover a cidade da tabela.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            lsbListagem.Items.Clear();

            if (tabela != null)
            {
                var conteudo = tabela.Conteudo();

                foreach (var cidade in conteudo)
                {
                    lsbListagem.Items.Add(cidade);
                }
            }
            else
            {
                MessageBox.Show("A tabela de hash está vazia. Não há cidades para listar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabela != null)
            {
                if (Salvar.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter arquivoSaida = new StreamWriter(Salvar.FileName))
                        {
                            var conteudo = tabela.Conteudo();
                            foreach (var cidade in conteudo)
                            {
                                cidade.ArmezenarDados(arquivoSaida);
                            }
                        }

                        MessageBox.Show("Dados salvos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocorreu um erro ao salvar os dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Não há dados para salvar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lsbListagem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

