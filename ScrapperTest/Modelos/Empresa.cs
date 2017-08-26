using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapperTest.Modelos
{
    public class Empresa
    {
        public string Nome { get; set; }

        public string NomeDePregao { get; set; }

        public string CodigoNegociacao { get; set; }

        public string CodigoISIN { get; set; }

        public string CodigoCVM { get; set; }

        public string CNPJ { get; set; }

        public string AtividadePrincipal { get; set; }

        public string ClassificacaoSetorial { get; set; }

        public string Site { get; set; }

        public string HtmlInformacoes { get; set; }

        public string Preco { get; set; }
        
        public string Variacao { get; set; }

        public List<PosicaoAcionaria> ListaPosicaoAcionaria { get; set; }

        public List<Acao> ListaAcoesEmCirculacao { get; set; }

        public CapitalSocial ComposicaoCapitalSocial { get; set; }
    }

    public class PosicaoAcionaria
    {
        public string Nome { get; set; }

        public string ON { get; set; }

        public string PN { get; set; }

        public string Total { get; set; }
    }

    public class Acao
    {
        public string TipoDeInvestidores { get; set; }

        public string Quantidade { get; set; }

        public string Percentual { get; set; }
    }

    public class CapitalSocial
    {
        public string Ordinarias { get; set; }

        public string Preferenciais { get; set; }
        
        public string Total { get; set; }
    }
}
