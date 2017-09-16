using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Net;
using System.IO.Compression;
using OpenQA.Selenium.Support.UI;
using BovespaBot.Modelos;

namespace BovespaBot.Metodos
{
    public class Bovespa
    {
        public static List<string> ConsultaEmpresas(List<string> listaCodigoCVM, List<Empresa> listaAtual, out List<Empresa> listaDeEmpresas)
        {
            List<string> codigosComErro = new List<string>();
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            listaDeEmpresas = new List<Empresa>();

            //2 Minutos e meio com boa taxa de sucesso. Realizar teste com 3 minutos
            foreach(var cod in listaCodigoCVM)
            {
                try{
                    Thread.Sleep(timeout);

                    var empresaAtualizada = RetornarDadosBasicos(cod);

                    if(empresaAtualizada == null)
                    {
                        codigosComErro.Add(cod);
                    }
                    else
                    {
                        var empresa = listaAtual.FirstOrDefault(c => c.CodigoCVM == cod);
                        if(empresa != null)
                        {
                            empresaAtualizada.Nome = empresa.Nome;
                            empresa = empresaAtualizada;
                        }
                        listaDeEmpresas.Add(empresa);
                    }
                }
                catch(Exception ex)
                {
                    codigosComErro.Add(cod);
                    continue;
                }
            }
            return codigosComErro;
        }

        public static List<Empresa> RetornaListaDeEmpresas()
        {
            List<Empresa> listaEmpresas = new List<Empresa>();

            using (var driver = new ChromeDriver())
            {
                driver.Url = "http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/BuscaEmpresaListada.aspx?idioma=pt-br";
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var btn = driver.FindElementById("ctl00_contentPlaceHolderConteudo_BuscaNomeEmpresa1_btnTodas");
                btn.Click();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                var table = driver.FindElementById("ctl00_contentPlaceHolderConteudo_BuscaNomeEmpresa1_grdEmpresa_ctl01");
                var trs = table.FindElements(By.CssSelector("tbody tr"));
                foreach (var tr in trs)
                {
                    var td = tr.FindElement(By.CssSelector("td"));
                    var a = td.FindElement(By.CssSelector("a"));
                    var nome = a.Text;
                    var html = a.GetAttribute("href");
                    var indice = html.LastIndexOf('=');
                    var codigo = html.Substring(indice + 1);
                    listaEmpresas.Add(new Empresa { Nome = nome, CodigoCVM = codigo });
                }
            }
            return listaEmpresas;
        }

        public static Empresa RetornarDadosBasicos(string codigoCVM)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");
            
            using (var driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Url = $"http://bvmf.bmfbovespa.com.br/pt-br/mercados/acoes/empresas/ExecutaAcaoConsultaInfoEmp.asp?CodCVM={codigoCVM}&ViewDoc=0#a";

                    if (driver.PageSource.Contains("Dados indisponiveis."))
                        throw new Exception("Código CVM não listado");

                    var empresa = new Empresa();

                    var divDados = driver.FindElementById("accordionDados");

                    if (divDados.Text != empresa.HtmlInformacoes)
                    {
                        foreach (var tr in divDados.FindElements(By.CssSelector("tr")))
                        {
                            var tds = tr.FindElements(By.CssSelector("td"));

                            switch (tds[0].Text)
                            {
                                case "Nome de Pregão:":
                                    {
                                        empresa.NomeDePregao = tds[1].Text;
                                        break;
                                    }
                                case "Códigos de Negociação:":
                                    {
                                        var a = tds[1].FindElement(By.TagName("a"));
                                        a.Click();
                                        var txtCodigos = tr.Text.Replace("\r\n", " ").Split();
                                        var j = 0;
                                        var listaNomeDeCodigos = new List<string> { "NEG", "ISIN", "N", "N" };
                                        empresa.CodigosNegociacao = new List<string>();
                                        empresa.CodigosISIN = new List<string>();
                                        var n = 5;
                                        if (tr.Text.Contains("Nenhum ativo no Mercado a Vista"))
                                        {
                                            if(txtCodigos[15] == "Códigos")
                                            {
                                                n = 17;
                                                j++;
                                            }
                                            else
                                            {
                                                n = 14;
                                            }
                                            
                                        }
                                        foreach (var txtCod in txtCodigos.Skip(n))
                                        {
                                            switch (listaNomeDeCodigos[j])
                                            {
                                                case "NEG":
                                                    {
                                                        if (txtCod != "ISIN:" && txtCod != "Códigos" && txtCod != "" && !empresa.CodigosNegociacao.Contains(txtCod.Replace(";", "")))
                                                            empresa.CodigosNegociacao.Add(txtCod.Replace(";", ""));
                                                        break;
                                                    }
                                                case "ISIN":
                                                    {
                                                        if (txtCod != "")
                                                            empresa.CodigosISIN.Add(txtCod.Replace(",", ""));
                                                        break;
                                                    }
                                                default:
                                                    break;
                                            }

                                            if (txtCod == "")
                                                j++;
                                            if (txtCod == "ISIN:")
                                                j++;
                                        }

                                        empresa.CodigoCVM = codigoCVM;
                                        break;
                                    }
                                case "CNPJ:":
                                    {
                                        empresa.CNPJ = tds[1].Text;
                                        break;
                                    }
                                case "Atividade Principal:":
                                    {
                                        empresa.AtividadePrincipal = tds[1].Text;
                                        break;
                                    }
                                case "Classificação Setorial:":
                                    {
                                        empresa.ClassificacaoSetorial = tds[1].Text;
                                        break;
                                    }
                                case "Site:":
                                    {
                                        empresa.Site = tds[1].Text;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }

                        empresa.HtmlInformacoes = divDados.Text;
                    }

                    //Ações
                    var divAcoes = driver.FindElementByTagName("iframe");
                    driver.SwitchTo().Frame(divAcoes);
                    var trsAcoes = driver.FindElementByCssSelector("tbody").FindElements(By.CssSelector("tr"));
                    empresa.ListaAcoes = new List<Acao>();
                    foreach (var tr in trsAcoes)
                    {
                        
                        empresa.ListaAcoes.Add(new Acao
                        {
                            CodigoDeNegociacao = tr.FindElement(By.ClassName("symbol-short-name-container")).Text,
                            NomeEmpresa = empresa.Nome,
                            Valor = tr.FindElement(By.ClassName("symbol-last")).Text,
                            Variacao = tr.FindElement(By.ClassName("symbol-change")).Text +" / " + tr.FindElement(By.ClassName("symbol-change-pt")).Text
                        });
                    }

                    //Posição Acionária
                    if(driver.PageSource.Contains("Posição Acionária"))
                    {
                        var divPosicaoAcionaria = driver.FindElementById("divPosicaoAcionaria");
                        var trsPosicaoAcionaria = divPosicaoAcionaria.FindElement(By.CssSelector("tbody")).FindElements(By.CssSelector("tr"));
                        empresa.ListaPosicaoAcionaria = new List<PosicaoAcionaria>();
                        foreach (var tr in trsPosicaoAcionaria)
                        {
                            var tds = tr.FindElements(By.CssSelector("td"));
                            empresa.ListaPosicaoAcionaria.Add(new PosicaoAcionaria
                            {
                                Nome = tds[0].Text,
                                ON = tds[1].Text,
                                PN = tds[2].Text,
                                Total = tds[3].Text
                            });
                        }
                    }
                    

                    //Ações Em Circulação
                    if(driver.PageSource.Contains("Ações em Circulação no Mercado"))
                    {
                        var divAcoesEmCirculacao = driver.FindElementById("div1");
                        var trsAcoesEmCirculacao = divAcoesEmCirculacao.FindElement(By.CssSelector("tbody")).FindElements(By.CssSelector("tr"));
                        empresa.ListaAcoesEmCirculacao = new List<AcoesEmCirculacao>();
                        foreach (var tr in trsAcoesEmCirculacao)
                        {
                            var tds = tr.FindElements(By.CssSelector("td"));
                            empresa.ListaAcoesEmCirculacao.Add(new AcoesEmCirculacao
                            {
                                TipoDeInvestidores = tds[0].Text,
                                Quantidade = tds[1].Text,
                                Percentual = tds[2].Text
                            });
                        }
                    }

                    //Composição do Capital Social
                    if (driver.PageSource.Contains("Composição do Capital Social"))
                    {
                        var divComposicaoCapitalSocial = driver.FindElementById("divComposicaoCapitalSocial");
                        var trsCapitalSocial = divComposicaoCapitalSocial.FindElement(By.CssSelector("tbody")).FindElements(By.CssSelector("tr"));
                        empresa.ComposicaoCapitalSocial = new CapitalSocial
                        {
                            Ordinarias = trsCapitalSocial[0].FindElement(By.ClassName("text-right")).Text,
                            Preferenciais = trsCapitalSocial[1].FindElement(By.ClassName("text-right")).Text,
                            Total = trsCapitalSocial[2].FindElement(By.ClassName("text-right")).Text
                        };
                    }

                    return empresa;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<Acao> RetornaValores(List<Acao> listaAcoes)
        {
            List<string> codigosComErro = new List<string>();
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            foreach (var acao in listaAcoes)
            {
                Thread.Sleep(timeout);
                    
                using (var driver = new ChromeDriver())
                {
                    try
                    {
                        driver.Url = $"https://www.tradingview.com/chart/{acao.CodigoDeNegociacao}/";
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        var x = driver.FindElementByClassName("tv-symbol-header-row__values-wrapper").Text;
                        var valores = x.Split('D');
                        var valor = valores[0];
                        var variacao = valores[1].Trim();
                        acao.Valor = valor;
                        acao.Variacao = variacao;
                    }
                    catch (Exception ex)
                    {
                        acao.Valor = "-";
                        acao.Variacao = "-";
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return listaAcoes;
        }

        public static string RetornaRelatorios(string codigoCVM, bool buscaHistorica)
        {
            //Relatórios Financeiros
            using (var driver = new ChromeDriver())
            {
                //Pegar ITR
                driver.Url = $"http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM={codigoCVM}&tipo=itr&ano=0";

                //Verificar por "Sistema Indisponível";
                var linhas = new List<IWebElement>();

                if (buscaHistorica)
                    linhas = driver.FindElementsByClassName("list-avatar-row").ToList();
                else
                    linhas = driver.FindElementsById("ctl00_contentPlaceHolderConteudo_rptDemonstrativo_ctl00_linha").ToList();

                foreach(var linha in linhas)
                {
                    var a = linha.FindElement(By.CssSelector("a"));
                    var nomeRelatorio = a.Text;
                    a.Click();

                    driver.SwitchTo().Window(driver.WindowHandles.Last());

                    var select1 = driver.FindElementById("ctl00_cphPopUp_cmbGrupo");

                    var selectElement = new SelectElement(select1);

                    selectElement.SelectByValue("24");

                    var x = driver.PageSource;
                    driver.SwitchTo().Frame("ctl00_cphPopUp_iFrameFormulariosFilho");
                    var setorAtividade = driver.FindElementById("ctl00_cphPopUp_txtSetorAtividade").Text;
                    driver.SwitchTo().DefaultContent();
                    select1 = driver.FindElementById("ctl00_cphPopUp_cmbGrupo");
                    selectElement = new SelectElement(select1);
                    selectElement.SelectByValue("78");
                    var dicBalancoPatrimonial = new Dictionary<string, string>();
                    driver.SwitchTo().Frame("ctl00_cphPopUp_iFrameFormulariosFilho");
                    var tabela = driver.FindElementById("ctl00_cphPopUp_tbDados").FindElements(By.CssSelector("tr"));

                    foreach (var item in tabela.Skip(1))
                    {

                        var tds = item.FindElements(By.CssSelector("td"));
                        var chave = tds[1].Text;
                        var valor = tds[2].Text;
                        if (!string.IsNullOrWhiteSpace(valor))
                            dicBalancoPatrimonial.Add(chave, valor);
                    }
                }

                //Se for buscar tudo:
                
                //Se buscar apenas o mais novo:
                

                //Pegar DFP
                //http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM=16284&tipo=itr&ano=0

                //http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM=16284&tipo=dfp&ano=0

                return "";
            }
        }
    }
}
