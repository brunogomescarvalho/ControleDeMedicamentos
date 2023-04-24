using System;
using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloPaciente
{
    public class TelaPaciente : Tela
    {
        RepositorioPaciente repositorioPaciente;
        CadastroEndereco cadastroEndereco;

        public TelaPaciente(RepositorioPaciente repositorioPaciente)
        {
            this.repositorioPaciente = repositorioPaciente;
            this.cadastroEndereco = new CadastroEndereco();
        }

        private Paciente SolicitarDados()
        {
            MostrarTexto("Informe o primeiro nome do paciente:");
            string nome = Console.ReadLine()!;

            MostrarTexto("Informe o sobrenome do paciente:");
            string sobreNome = Console.ReadLine()!;

            MostrarTexto("Data de nascimento: (dd/MM/yyyy)");
            string dataNasci = Console.ReadLine()!;
            DateTime dataNascimento = ValidarData(dataNasci) ? Convert.ToDateTime(dataNasci) : default;

            MostrarTexto("Nr Cartão saúde:");
            string nrCartao = Console.ReadLine()!;

            MostrarTexto("Telefone:");
            string telefone = Console.ReadLine()!;

            Endereco endereco = cadastroEndereco.CadastrarEndereco();
            Paciente paciente = new Paciente(nome, sobreNome, dataNascimento, nrCartao, telefone, endereco);
            
            bool pacienteValido = ValidarDados(repositorioPaciente.ObterValoresPropriedades(paciente));
            bool enderecoValido = ValidarDados(repositorioPaciente.ObterValoresPropriedades(endereco));

            return pacienteValido && enderecoValido ? paciente : null!;

        }

        protected override void Cadastrar()
        {
            Paciente paciente = SolicitarDados();

            if (paciente == null)
                return;

            this.repositorioPaciente.Adicionar(paciente);
            MostrarMensagemStatus(ConsoleColor.Green, "Paciente cadastrado com sucesso!");
        }

        protected override void Editar()
        {

            MostrarTexto("--- Editar Paciente ---");
            ArrayList pacientes = repositorioPaciente.BuscarTodos();

            if (VerificarListaContemItens(pacientes, "pacientes"))
                RenderizarTabela(pacientes, false);

            Console.Write("\nInforme o id do paciente: ");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
                return;

            Paciente paciente = (Paciente)repositorioPaciente.BuscarPorId(int.Parse(id));

            if (paciente == null)
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Id não localizado");
                return;
            }
            Paciente pacienteEditado = SolicitarDados();

            if (pacienteEditado == null)
            {
                return;
            }

            paciente.Editar(pacienteEditado);

            MostrarMensagemStatus(ConsoleColor.Green, "Paciente editado com sucesso!");
        }

        protected override void Visualizar()
        {
            MostrarTexto("--- Pacientes Cadastrados ---");
            ArrayList pacientes = repositorioPaciente.BuscarTodos();

            if (VerificarListaContemItens(pacientes, "pacientes"))
                RenderizarTabela(pacientes, true);
        }

        protected override void RenderizarTabela(ArrayList pacientes, bool esperarTecla)
        {
            ExibirCabecalhoTabela();
            base.RenderizarTabela(pacientes, esperarTecla);
        }

        private void ExibirCabecalhoTabela()
        {
            Console.WriteLine($"\n{"ID",-3} | {"NOME COMPLETO",-20} | {"IDADE",-5} | {"CARTÃO SAÚDE",-12} | {"TELEFONE",-12} | {cadastroEndereco.MostrarCabecalho()}");
            Console.WriteLine("----|----------------------|-------|--------------|--------------|---------------------------|-------|---------------------------|------------|---------------");
        }
    }
}