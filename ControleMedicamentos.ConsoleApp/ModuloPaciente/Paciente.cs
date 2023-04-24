using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    public class Paciente : Entidade
    {
        private string sobrenome { get; set; }
        public DateTime dataNascimento { get; private set; }
        public int idade { get => DateTime.Now.Year - this.dataNascimento.Year; }
        public string nomeCompleto { get => $"{nome} {sobrenome}"; }
        public string cartaoDeSaude { get; private set; }
        public string telefone { get; private set; }
        public Endereco endereco { get; private set; }

        public Paciente(string nome,
        string sobrenome,
        DateTime dataNasc,
        string cartaoDeSaude,
        string telefone,
        Endereco endereco) : base(nome)
        {
            this.sobrenome = sobrenome;
            this.dataNascimento = dataNasc;
            this.cartaoDeSaude = cartaoDeSaude;
            this.telefone = telefone;
            this.endereco = endereco;
        }

        public override void Editar(Entidade entidade)
        {
            Paciente paciente = (Paciente)entidade;
            this.dataNascimento = paciente.dataNascimento;
            this.cartaoDeSaude = paciente.cartaoDeSaude;
            this.telefone = paciente.telefone;
            this.endereco = paciente.endereco;
        }

        public override string ToString()
        {
            return $"{id,-3} | {nomeCompleto,-20} | {idade,-5} | {cartaoDeSaude,-12} | {telefone,-12} | {endereco}";
        }
    }
}