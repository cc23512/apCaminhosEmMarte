using System;
using System.IO;

namespace apCaminhosEmMarte
{
    public class Cidade : IRegistro<Cidade>
    {
        const int tamanhoMaximoX = 7, tamanhoMaximoY = 7, posicaoInicialNome = 0, tamanhoMaximoNome = 15,
        posicaoInicialY = posicaoInicialX + tamanhoMaximoX,
        posicaoInicialX = posicaoInicialNome + tamanhoMaximoNome;
          
        string nomeCidade;
        double x, y;
        public Cidade()
        {
            // contrutor padrao
        }
        public string Chave => NomeCidade;
        public string NomeCidade
        {
            get => nomeCidade;
            set { nomeCidade = value.PadRight(tamanhoMaximoNome, ' ').Substring(0, tamanhoMaximoNome); }
        }

        public double Y
        {
            get => y;
            set
            {
                if (value < 0 || value > 1)
                    throw new Exception("y não nao esta entre 0 ou 1");
                y = value;
            }
        }

        public double X
        {
            get => x;
            set
            {
                if (value < 0 || value > 1)
                    throw new Exception("x nao esta entre o ou 1");
                x = value;
            }
        }

        public void ArmezenarDados(StreamWriter arquivo)
        {
            if (arquivo != null) 
            {
                if (X < 0 || X > 1 || Y < 0 || Y > 1)
                    throw new Exception("valores de X e/ou Y estão fora do intervalo permitido.");

                arquivo.WriteLine($"{NomeCidade}{X:0.00000}{Y:0.00000}");
            }
        }

        public void LerRegistro(StreamReader arquivo)  
        {
            if (arquivo != null) 
            {
                if (!arquivo.EndOfStream) 
                {
                    string linhaLida = arquivo.ReadLine();
                    NomeCidade = linhaLida.Substring(posicaoInicialNome, tamanhoMaximoNome);
                    string strX = linhaLida.Substring(posicaoInicialX, tamanhoMaximoX);
                    string strY = linhaLida.Substring(posicaoInicialY, tamanhoMaximoY);
                    // converte x e y
                    if (!double.TryParse(strX, out x) || !double.TryParse(strY, out y))
                        throw new Exception("Erro ao converter valores de X e/ou Y para double.");
                }
            }
        }

        public int CompareTo(Cidade outra) 
        {
            return this.nomeCidade.CompareTo(outra.nomeCidade);
        }

        public override string ToString()
        {
            return NomeCidade + " " + X + " " + Y;
        }
    }
}
