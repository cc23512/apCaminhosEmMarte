using System;
using System.Collections.Generic;

namespace apCaminhosEmMarte
{
    public class HashLinear<Tipo> : ITabelaDeHash<Tipo>
        where Tipo : IRegistro<Tipo>
    {
        private const int TamanhoTabela = 131;
        private Tipo[] tabela;

        public HashLinear()
        {
            tabela = new Tipo[TamanhoTabela];
        }

        public int FuncaoHash(string chave)
        {
            long hashTotal = 0;
            foreach (char c in chave)
                hashTotal += 37 * hashTotal + (char)c;

            hashTotal = hashTotal % tabela.Length;
            if (hashTotal < 0)
            {
                hashTotal += tabela.Length;
            }
            return (int)hashTotal;
        }

        public void Inserir(Tipo item)
        {
            int indice = FuncaoHash(item.Chave);

            while (tabela[indice] != null)
            {
                indice = (indice + 1) % TamanhoTabela;
            }

            tabela[indice] = item;
        }

        public bool Remover(Tipo item)
        {
            int indice = FuncaoHash(item.Chave);

            while (tabela[indice] != null)
            {
                if (tabela[indice].Equals(item))
                {
                    tabela[indice] = default(Tipo);
                    return true;
                }
                indice = (indice + 1) % TamanhoTabela;
            }

            return false;
        }

        public bool Existe(Tipo item, out int posicao)
        {
            int indice = FuncaoHash(item.Chave);

            while (tabela[indice] != null)
            {
                if (tabela[indice].Equals(item))
                {
                    posicao = indice;
                    return true;
                }
                indice = (indice + 1) % TamanhoTabela;
            }

            posicao = -1;
            return false;
        }

        public List<Tipo> Conteudo()
        {
            List<Tipo> conteudo = new List<Tipo>();
            foreach (var item in tabela)
            {
                if (item != null)
                {
                    conteudo.Add(item);
                }
            }
            return conteudo;
        }
    }
}
