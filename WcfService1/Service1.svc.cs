using ScrapperTest;
using ScrapperTest.Metodos;
using ScrapperTest.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        //public bool ConsultaPorTransportador(string rntrc, string cpf)
        //{
        //    return Antt.ConsultaPorTransportador(rntrc, cpf);
        //}

        public bool ConsultaCadastroCPF(string cpf, string dataNascimento)
        {
            return Receita.ConsultaCadastroCPF(cpf, dataNascimento);
        }

        //public bool ConsultaCadastroCNPJ(string cnpj)
        //{
        //    return Receita.ConsultaCadastroCNPJ(cnpj);
        //}

        //public bool ConsultaPontos(string cpf, string cnh, string uf)
        //{
        //    return Detran.ConsultaPontos(cpf, cnh, uf);
        //}

        //public List<MultaRenavamModel> ConsultaNadaConsta(string renavam)
        //{
        //    return Detran.ConsultaNadaConsta(renavam);
        //}

        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public CompositeType GetDataUsingDataContract(CompositeType composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}
    }
}
