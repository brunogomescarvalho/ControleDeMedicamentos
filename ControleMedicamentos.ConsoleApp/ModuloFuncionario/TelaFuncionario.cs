using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloFuncionario
{
    public class TelaFuncionario : Tela
    {
        RepositorioFuncionario repositorioFuncionario;
        CadastroEndereco cadastroEndereco;
        
        public TelaFuncionario(RepositorioFuncionario repositorioFuncionario)
        {
            this.repositorioFuncionario = repositorioFuncionario;
            this.cadastroEndereco = new CadastroEndereco();
        }

        protected override void Cadastrar()
        {
            MostrarTexto("--- Cadastrar Funcionário ---\n");

            Funcionario funcionario = SolicitarDados();

            if (funcionario == null!)
                return;

            repositorioFuncionario.Adicionar(funcionario);
            MostrarMensagemStatus(ConsoleColor.Green, "Cadastro Funcionário Efetuado com Sucesso");
        }

        protected override void Visualizar()
        {
            MostrarTexto("--- Funcionários Cadastrados ---");
            ArrayList funcionarios = repositorioFuncionario.BuscarTodos();

            if (VerificarListaContemItens(funcionarios, "funcionários"))
                foreach (Funcionario item in funcionarios)
                {
                    Console.WriteLine($"{item.id,-5} | {item.nome,-20} | {item.codigo,-5} | {item.telefone,-12} | {item.endereco}");
                }
            Console.ReadKey();
        }

        protected override void Editar()
        {
            MostrarTexto("--- Editar Funcionário ---");
            ArrayList funcionarios = repositorioFuncionario.BuscarTodos();

            if (VerificarListaContemItens(funcionarios, "funcionário"))
                RenderizarTabela(funcionarios, false);

            Console.WriteLine("\nInforme o id do Funcionário");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Id não localizado");
                return;
            }

            Funcionario Funcionario = (Funcionario)repositorioFuncionario.BuscarPorId(int.Parse(id));

            Funcionario atualizado = SolicitarDados();

            if (atualizado == null)
                return;

            Funcionario.Editar(atualizado);

            MostrarMensagemStatus(ConsoleColor.Green, "Cadastro editado com sucesso");
        }


        private Funcionario SolicitarDados()
        {
            MostrarTexto("Informe o Nome");
            string nome = Console.ReadLine()!;

            MostrarTexto("Informe o Cpf:");
            string cpf = Console.ReadLine()!;

            MostrarTexto("Informe o Telefone:");
            string telefone = Console.ReadLine()!;

            Endereco endereco = this.cadastroEndereco.CadastrarEndereco();
            Funcionario funcionario = new Funcionario(nome, cpf, telefone, endereco);

            bool funcionarioValido = ValidarDados(repositorioFuncionario.ObterValoresPropriedades(funcionario));
            bool enderecoValido = ValidarDados(repositorioFuncionario.ObterValoresPropriedades(endereco));
            
            return funcionarioValido && enderecoValido ? funcionario : null!;
        }
    }
}