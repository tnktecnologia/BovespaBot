using BovespaBot.Extensions;
using BovespaBot.Metodos;
using BovespaBot.Modelos;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace BovespaBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            CarregarListaDeEmpresas();
        }

        private List<Empresa> listaEmpresa { get; set; }

        private List<Empresa> empresasChecked {
            get
            {
                return  chkListEmpresas.CheckedItems.Cast<Empresa>().ToList();
            }
        }

        private string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\BovespaBot";

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var listaAcoes = new List<Acao>();
                var listaTemp = new List<Empresa>();
                var listaCodigos = new List<string>();
                var empresasABuscar = empresasChecked;
                if (chkDados.Checked)
                {
                    listaCodigos = empresasABuscar.Select(c => c.CodigoCVM).ToList();
                    while (listaCodigos.Count > 0)
                    {
                        var listaErros = Bovespa.ConsultaEmpresas(listaCodigos, empresasABuscar, out listaTemp);
                        listaCodigos = listaErros;

                        foreach(var emp in listaTemp)
                        {
                            try
                            {
                                var i = empresasABuscar.IndexOf(empresasABuscar.First(c => c.CodigoCVM == emp.CodigoCVM));
                                empresasABuscar[i] = emp;
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }

                //Consulta Relatórios financeiros
                
                if (chkRelatorios.Checked)
                {
                    foreach(var codigo in listaCodigos)
                    {
                        var listaErros = Bovespa.RetornaRelatorios(codigo, true);
                    }
                }

                ExportToExcel(empresasABuscar);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ExportListaEmpresa(List<Empresa> listaEmpresa)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            path += $"\\Empresas";

            var listaTabelas = new List<System.Data.DataTable>();

            var dadosEmpresa = new System.Data.DataTable("Dados de Empresas");

            dadosEmpresa.Columns.Add("Nome", typeof(string));
            dadosEmpresa.Columns.Add("Codigo CVM", typeof(string));

            foreach (var empresa in listaEmpresa)
            {
                dadosEmpresa.Rows.Add(empresa.Nome,
                    empresa.CodigoCVM);
            }

            listaTabelas.Add(dadosEmpresa);

            listaTabelas.ExportToExcel(path);
        }

        public void ExportToExcel(List<Empresa> listaEmpresa)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var newPath = path +  $"\\RelatorioBovespa__{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}";

            

            var listaTabelas = new List<System.Data.DataTable>();

            var dadosEmpresa = new System.Data.DataTable("Dados de Empresas");

            dadosEmpresa.Columns.Add("Nome", typeof(string));
            dadosEmpresa.Columns.Add("Nome do Pregão", typeof(string));
            dadosEmpresa.Columns.Add("Codigos de Negociação", typeof(string));
            dadosEmpresa.Columns.Add("Codigos ISIN", typeof(string));
            dadosEmpresa.Columns.Add("Codigo CVM", typeof(string));
            dadosEmpresa.Columns.Add("CNPJ", typeof(string));
            dadosEmpresa.Columns.Add("Atividade Principal", typeof(string));
            dadosEmpresa.Columns.Add("Classificação  Setorial", typeof(string));
            dadosEmpresa.Columns.Add("Site", typeof(string));
            dadosEmpresa.Columns.Add("Posição Acionária", typeof(string));
            dadosEmpresa.Columns.Add("Ações em Circulação", typeof(string));
            dadosEmpresa.Columns.Add("Composição do Capital Social", typeof(string));

            var acoesEmpresa = new System.Data.DataTable("Ações");

            acoesEmpresa.Columns.Add("Nome", typeof(string));
            acoesEmpresa.Columns.Add("Código De Negociação", typeof(string));
            acoesEmpresa.Columns.Add("Valor", typeof(string));
            acoesEmpresa.Columns.Add("Variação", typeof(string));

            foreach (var empresa in listaEmpresa)
            {
                dadosEmpresa.Rows.Add(empresa.Nome, 
                    empresa.NomeDePregao,
                    empresa.CodigosNegociacao.Count>0 ?  string.Join(",",  empresa.CodigosNegociacao) : "",
                    empresa.CodigosISIN.Count>0 ? string.Join(",", empresa.CodigosISIN) : "", 
                    empresa.CodigoCVM, 
                    empresa.CNPJ, 
                    empresa.AtividadePrincipal, 
                    empresa.ClassificacaoSetorial, 
                    empresa.Site,
                    String.Join("||", empresa.ListaPosicaoAcionaria.Select(c=> "Nome: " + c.Nome + "-- %ON: " + c.ON + "-- %PN: " + c.PN + "-- %Total: " +c.Total)),
                    String.Join("||", empresa.ListaAcoesEmCirculacao.Select(c => "Investidor: " + c.TipoDeInvestidores + "-- Quantidade: " + c.Quantidade + "-- Percentual: " + c.Percentual)),
                    String.Join("||", "Ordinárias: " + empresa.ComposicaoCapitalSocial.Ordinarias + "-- Preferenciais: " + empresa.ComposicaoCapitalSocial.Preferenciais + "-- Total:" + empresa.ComposicaoCapitalSocial.Total));

                foreach(var acao in empresa.ListaAcoes)
                {
                    acoesEmpresa.Rows.Add(acao.NomeEmpresa, acao.CodigoDeNegociacao, acao.Valor, acao.Variacao);
                }
            }


            listaTabelas.Add(acoesEmpresa);
            listaTabelas.Add(dadosEmpresa);

            listaTabelas.ExportToExcel(newPath);
        }

        public List<Empresa> ImportFromExcel()
        {
            if (!Directory.Exists(path))
                throw new Exception("Caminho não encontrado");

            var directory = new DirectoryInfo(path);
            var arquivo = (from f in directory.GetFiles().Where(c=> c.Name == "Empresas.xlsx")
                           orderby f.LastWriteTime descending
                           select f).First();

            lblAtualizada.Text = "Última Atualização em " + arquivo.LastWriteTime.ToShortDateString() + " às " + arquivo.LastWriteTime.ToShortTimeString();
            var listaEmpresa = new List<Empresa>();

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(arquivo.FullName, ReadOnly: true);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            try
            {
                for (int i = 2; i <= xlRange.Rows.Count; i++)
                {
                    var empresa = new Empresa();

                    empresa.Nome = xlRange.Cells[i, 1].Value2.ToString();
                    empresa.CodigoCVM = xlRange.Cells[i, 2].Value2.ToString();
                    listaEmpresa.Add(empresa);
                }
            }
            catch
            {
                Console.WriteLine("Erro na importação");
            }
            finally
            {
                xlWorkbook.Close(0);
                xlApp.Quit();
                
            }
            return listaEmpresa;
        }

        #region Métodos
        
        private List<Acao> VerificaAcoes(List<Empresa> listaEmpresa, List<Acao> listaAcoes)
        {
            foreach (var empresa in listaEmpresa)
            {
                listaAcoes.AddRange(empresa.CodigosNegociacao.Select(c => new Acao
                {
                    CodigoDeNegociacao = c,
                    NomeEmpresa = empresa.Nome
                }));
            }
            var listaAux = new List<Acao>();
            var listaCodigosNeg = listaEmpresa.SelectMany(c => c.CodigosNegociacao).ToList();
            return Bovespa.RetornaValores(listaAcoes);
        }

        #endregion

        private void chkDados_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkValores_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkRelatorios_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkSelecionarTodas_CheckedChanged(object sender, EventArgs e)
        {
            for (int ix = 0; ix < chkListEmpresas.Items.Count; ++ix)
                chkListEmpresas.SetItemChecked(ix, chkSelecionarTodas.Checked);
        }

        private void CarregarListaDeEmpresas()
        {
            if(listaEmpresa != null && listaEmpresa.Count > 0)
                chkListEmpresas.Items.AddRange(listaEmpresa.ToArray());
            else
            {
                try
                {
                    listaEmpresa = ImportFromExcel(); //Carregar listaEmpresa com dados do ultimo Excel gerado
                    chkListEmpresas.Items.AddRange(listaEmpresa.ToArray());
                }
                catch
                {
                    chkListEmpresas.Items.Add("Clique em Consultar Atualizar Lista de Empresas para obter a lista de empresas atualizada.");
                }
            }
            
        }

        private void SalvarEmpresasAtualizadas()
        {
            ExportListaEmpresa(listaEmpresa);
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listaEmpresa = Bovespa.RetornaListaDeEmpresas();
            chkListEmpresas.Items.Clear();
            SalvarEmpresasAtualizadas();
            CarregarListaDeEmpresas();
            lblAtualizada.Text = "Última Atualização em " + DateTime.Today.ToShortDateString() + " às " + DateTime.Today.ToShortTimeString();
        }
    }
}
