using System;
using System.Collections.Generic;

namespace apCaminhosEmMarte
{
    class HashDuplo<Tipo> : ITabelaDeHash<Tipo>
        where Tipo : IRegistro<Tipo>
    {
        private const int TamanhoTabela = 131;
        private Tipo[] tabela;

        public HashDuplo()
        {
            tabela = new Tipo[TamanhoTabela];
        }

        public int Hash1(string chave)
        {
            long hashTotal = 0;
            foreach (char c in chave)
                hashTotal += 37 * hashTotal + (char)c;

            hashTotal = hashTotal % tabela.Length;
            if (hashTotal < 0)
                hashTotal += tabela.Length;
            return (int)hashTotal;
        }

        public int Hash2(string chave)
        {
            return 1 + (chave.Length % (TamanhoTabela - 1));
        }

        public void Inserir(Tipo item)
        {
            int hash1 = Hash1(item.Chave);
            int hash2 = Hash2(item.Chave);
            int indice = hash1;

            for (int i = 1; tabela[indice] != null; i++)
            {
                indice = (hash1 + i * hash2) % TamanhoTabela;
            }

            tabela[indice] = item;
        }

        public bool Remover(Tipo item)
        {
            int hash1 = Hash1(item.Chave);
            int hash2 = Hash2(item.Chave);
            int indice = hash1;

            for (int i = 1; tabela[indice] != null; i++)
            {
                if (tabela[indice].Equals(item))
                {
                    tabela[indice] = default(Tipo);
                    return true;
                }
                indice = (hash1 + i * hash2) % TamanhoTabela;
            }

            return false;
        }

        public bool Existe(Tipo item, out int posicao)
        {
            int hash1 = Hash1(item.Chave);
            int hash2 = Hash2(item.Chave);
            int indice = hash1;

            for (int i = 1; tabela[indice] != null; i++)
            {
                if (tabela[indice].Equals(item))
                {
                    posicao = indice;
                    return true;
                }
                indice = (hash1 + i * hash2) % TamanhoTabela;
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
