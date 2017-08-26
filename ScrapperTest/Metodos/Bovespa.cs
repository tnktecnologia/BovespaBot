using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatiN.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using ScrapperTest.Modelos;
using System.Net;
using System.IO.Compression;
using OpenQA.Selenium.Support.UI;

namespace ScrapperTest.Metodos
{
    public class Bovespa
    {
        public static void ConsultaEmpresas(List<string> listaCVM)
        {
            List<string> codigosComErro = new List<string>();
            List<Empresa> listaDeEmpresas = new List<Empresa>();
            List<string> listaCodigoCVM = new List<string>();
            TimeSpan timeout = TimeSpan.FromMinutes(3);

            using (var driver = new ChromeDriver())
            {
                driver.Url = "http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/BuscaEmpresaListada.aspx?idioma=pt-br";
                var btn = driver.FindElementById("ctl00_contentPlaceHolderConteudo_BuscaNomeEmpresa1_btnTodas");
                btn.Click();
                var table = driver.FindElementById("ctl00_contentPlaceHolderConteudo_BuscaNomeEmpresa1_grdEmpresa_ctl01");
                var trs = table.FindElements(By.CssSelector("tbody tr"));
                foreach(var tr in trs)
                {
                    var td = tr.FindElement(By.CssSelector("td"));
                    var html = td.FindElement(By.CssSelector("a")).GetAttribute("href");
                    var indice = html.LastIndexOf('=');
                    var codigo = html.Substring(indice+1);
                    listaCodigoCVM.Add(codigo);
                    Console.WriteLine(html);
                }

                //2 Minutos e meio com boa taxa de sucesso. Realizar teste com 3 minutos
                foreach(var cod in listaCodigoCVM)
                {
                    try{
                        Thread.Sleep(timeout);

                        var empresa = RetornarDadosBasicos(cod);

                        if(empresa == null)
                        {
                            codigosComErro.Add(cod);
                        }
                        else
                        {
                            listaDeEmpresas.Add(empresa);
                        }
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                    

                    //empresa = RetornarDadosBasicos(cod);

                    //if (empresa == null)
                    //    continue; // Tratar erro

                    //Retornar Variação e preço
                    //var variacaoEPreco = RetornarVariacaoEPreco(empresa.CodigoNegociacao);

                    //Retornar Relatórios
                    //var relatorios = RetornaRelatorios(cod);

                    //Salvar empresa no banco - atualizar data de consulta - etc
                }
            }
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

                                        empresa.CodigoNegociacao = txtCodigos[5];
                                        empresa.CodigoISIN = txtCodigos[9];
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

                    //Posição Acionária
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

                    //Ações Em Circulação
                    var divAcoesEmCirculacao = driver.FindElementById("div1");
                    var trsAcoesEmCirculacao = divAcoesEmCirculacao.FindElement(By.CssSelector("tbody")).FindElements(By.CssSelector("tr"));
                    empresa.ListaAcoesEmCirculacao = new List<Acao>();
                    foreach (var tr in trsAcoesEmCirculacao)
                    {
                        var tds = tr.FindElements(By.CssSelector("td"));
                        empresa.ListaAcoesEmCirculacao.Add(new Acao
                        {
                            TipoDeInvestidores = tds[0].Text,
                            Quantidade = tds[1].Text,
                            Percentual = tds[2].Text
                        });
                    }

                    //Composição do Capital Social
                    var divComposicaoCapitalSocial = driver.FindElementById("divComposicaoCapitalSocial");
                    var trsCapitalSocial = divComposicaoCapitalSocial.FindElement(By.CssSelector("tbody")).FindElements(By.CssSelector("tr"));
                    empresa.ComposicaoCapitalSocial = new CapitalSocial
                    {
                        Ordinarias = trsCapitalSocial[0].FindElement(By.ClassName("text-right")).Text,
                        Preferenciais = trsCapitalSocial[1].FindElement(By.ClassName("text-right")).Text,
                        Total = trsCapitalSocial[2].FindElement(By.ClassName("text-right")).Text
                    };

                    return empresa;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static string RetornarVariacaoEPreco(string codigoNegociacao)
        {
            // Buscar Preço e Variação no site Trading View
            using (var driver2 = new ChromeDriver())
            {
                try
                {
                    driver2.Url = $"https://www.tradingview.com/chart/{codigoNegociacao}/";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return driver2.FindElementByClassName("tv-symbol-header-row__values-wrapper").Text;
            }
        }

        public static string RetornaRelatorios(string codigoCVM)
        {
            //Relatórios Financeiros
            using (var driver = new ChromeDriver())
            {
                //Pegar ITR
                //driver.Url = $"http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM={codigoCVM}&tipo=itr&ano=0";
                driver.Url = "http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/BuscaEmpresaListada.aspx?idioma=pt-br";
                

                //Verificar por "Sistema Indisponível";

                var divNova = driver.FindElementById("ctl00_contentPlaceHolderConteudo_rptDemonstrativo_ctl00_linha");
                var a = divNova.FindElement(By.CssSelector("a"));
                a.Click();

                driver.SwitchTo().Window(driver.WindowHandles.Last());

                var select1 = driver.FindElementById("ctl00_cphPopUp_cmbGrupo");

                var selectElement = new SelectElement(select1);

                selectElement.SelectByValue("24");

                driver.FindElementById("ctl00_cphPopUp_tabMenuModelo_tabItem3").Click();

                //Pegar DFP
                //http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM=16284&tipo=itr&ano=0

                //http://bvmf.bmfbovespa.com.br/cias-listadas/empresas-listadas/HistoricoFormularioReferencia.aspx?codigoCVM=16284&tipo=dfp&ano=0

                return "";
            }
        }
    }
}
