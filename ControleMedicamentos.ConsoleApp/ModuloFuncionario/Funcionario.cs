using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloFuncionario
{
    public class Funcionario : Entidade
    {
        public string cpf { get; private set; }
        public string codigo { get; private set; }
        public string telefone { get; private set; }
        public Endereco endereco { get; private set; }

        public Funcionario(string nome, string cpf, string telefone, Endereco endereco) : base(nome)
        {
            this.cpf = cpf;
            this.codigo =  GerarCodigo();
            this.telefone = telefone;
            this.endereco = endereco;
        }

        public override void Editar(Entidade entidade)
        {
            Funcionario funcionario = (Funcionario)entidade;

            this.nome = funcionario.nome;
            this.cpf = funcionario.cpf;
            this.telefone = funcionario.telefone;
            this.endereco = funcionario.endereco;
        }

        public string GerarCodigo()
        {
            Random random = new Random();
            string caracteres = "0123456789ABCDEFG";
            string codigo = "";

            while (codigo.Length < 4)
            {
                codigo += caracteres[random.Next(caracteres.Length)];
            }
            return codigo;

        }

        public override string ToString()
        {
            return $"{id,-5} | {nome,-20} | {codigo,-5}";
        }
    }
}