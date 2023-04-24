using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao
{
    public class Requisicao : Entidade
    {
        public DateTime dataDeEmissao { get; private set; }
        public Medicamento medicamento { get; private set; }

        public int quantidade { get; private set; }
        public Paciente paciente { get; private set; }
        public Funcionario funcionario { get; private set; }
        public string crmMedico { get; private set; }


        public Requisicao(Medicamento medicamento, int quantidade, Paciente paciente, DateTime dataEmissao, string crmMedico, Funcionario funcionario)
        {
            this.medicamento = medicamento;
            this.quantidade = quantidade;
            this.paciente = paciente;
            this.dataDeEmissao = dataEmissao;
            this.crmMedico = crmMedico;
            this.funcionario = funcionario;
        }

        public void Editar(Requisicao requisicao)
        {
            this.medicamento = requisicao.medicamento;
            this.quantidade = requisicao.quantidade;
            this.paciente = requisicao.paciente;
            this.dataDeEmissao = requisicao.dataDeEmissao;
            this.crmMedico = requisicao.crmMedico;
            this.funcionario = requisicao.funcionario;
        }
        public override string ToString()
        {
            return $"{id,-3} | {medicamento.nome,-20} {medicamento.id,-3} | {quantidade,-4} | {paciente.nomeCompleto,-20} | {paciente.cartaoDeSaude,-12} | {dataDeEmissao,-15:d} | {crmMedico,-8} | {funcionario.nome,-15} {funcionario.codigo,5}";
        }
    }
}