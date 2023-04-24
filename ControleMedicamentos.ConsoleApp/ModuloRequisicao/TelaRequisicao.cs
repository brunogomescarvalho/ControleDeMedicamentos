using System.Linq;
using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;

namespace ControleMedicamentos.ConsoleApp.ModuloRequisicao;

public class TelaRequisicao : Tela
{
    private readonly RepositorioMedicamento repositorioMedicamento;
    private readonly RepositorioPaciente repositorioPaciente;
    private readonly RepositorioFuncionario repositorioFuncionario;
    private readonly RepositorioRequisicao repositorioRequisicao;

    public TelaRequisicao(
        RepositorioMedicamento repositorioMedicamento,
        RepositorioPaciente repositorioPaciente,
        RepositorioFuncionario repositorioFuncionario,
        RepositorioRequisicao repositorioRequisicao
      )
    {
        this.repositorioFuncionario = repositorioFuncionario;
        this.repositorioMedicamento = repositorioMedicamento;
        this.repositorioPaciente = repositorioPaciente;
        this.repositorioRequisicao = repositorioRequisicao;
    }

    protected override void Cadastrar()
    {
        Requisicao requisicao = SolicitarDados();
        if (requisicao == null)
            return;

        if (!requisicao.medicamento.DeduzirQuantidade(requisicao.quantidade))
        {
            MostrarMensagemStatus(ConsoleColor.Yellow, $"A Quantidade em estoque do medicamento é insuficiente. Total {requisicao.medicamento.quantidade}");
            return;
        }
        repositorioRequisicao.Adicionar(requisicao);
        MostrarMensagemStatus(ConsoleColor.Green, "Requisição cadastrada com sucesso");

    }


    protected override void Editar()
    {

        ArrayList requisicoes = repositorioRequisicao.BuscarTodos();

        if (!VerificarListaContemItens(requisicoes, "requisições"))
            return;
        ExibirCabecalhoRequisicao();
        RenderizarTabela(requisicoes, false);

        System.Console.WriteLine("\n Informe o id da Requisição para editar");

        string id = Console.ReadLine()!;

        if (!OpcaoValida(id))
            return;

        Requisicao requisicao = (Requisicao)repositorioRequisicao.BuscarPorId(int.Parse(id));

        if (!VerificarItemEncontrado(requisicao, "Requisição informada não localizada"))
        {
            return;
        }

        Requisicao editada = SolicitarDados();

        if (editada == null)
            return;

        if (!editada.medicamento.DeduzirQuantidade(editada.quantidade))
        {
            MostrarMensagemStatus(ConsoleColor.Yellow, $"A Quantidade em estoque do medicamento é insuficiente. Total {requisicao.medicamento.quantidade}");
            return;
        }

        requisicao.medicamento.AdicionarQuantidade(requisicao.quantidade);
        requisicao.Editar(editada);
        MostrarMensagemStatus(ConsoleColor.Green, "Requisição editada com sucesso");
    }



    private Requisicao SolicitarDados()
    {

        ArrayList pacientes = repositorioPaciente.BuscarTodos();
        ArrayList funcionarios = repositorioFuncionario.BuscarTodos();
        ArrayList medicamentos = repositorioMedicamento.BuscarTodos();

        if (!VerificarListaContemItens(medicamentos, "medicamentos") ||
        !VerificarListaContemItens(funcionarios, "funcionarios") ||
        !VerificarListaContemItens(pacientes, "pacientes"))
            return null!;

        Console.Clear();
        ExibirCabecalhoPaciente();
        MostrarLista(pacientes);

        Console.WriteLine("\nInforme o Id do paciente");
        string idPaciente = Console.ReadLine()!;

        if (!OpcaoValida(idPaciente))
            return null!;

        Paciente paciente = (Paciente)repositorioPaciente.BuscarPorId(int.Parse(idPaciente));

        if (!VerificarItemEncontrado(paciente, "Paciente não localizado"))
            return null!;

        ExibirTabelaMedicamento();

        Console.WriteLine("\nInforme o Id do Medicamento");
        string id = Console.ReadLine()!;

        if (!OpcaoValida(id))
        {
            MostrarMensagemStatus(ConsoleColor.Red, "Opção inválida");
            return null!;
        }

        Medicamento medicamento = (Medicamento)repositorioMedicamento.BuscarPorId(int.Parse(id));

        if (!VerificarItemEncontrado(medicamento, "Medicamento não localizado"))
            return null!;

        MostrarTexto("Informe a quantidade");
        string qtd = Console.ReadLine()!;

        if (!OpcaoValida(qtd) || int.Parse(qtd) <= 0)
        {
            MostrarMensagemStatus(ConsoleColor.Red, "Quantidade informada inválida");
            return null!;
        }

        MostrarTexto("Informe a data de emissão da requisição");
        string data = Console.ReadLine()!;
        DateTime dataRequisicao = ValidarData(data) ? Convert.ToDateTime(data) : default;

        MostrarTexto("Informe o Crm do Médico solicitante");
        string crm = Console.ReadLine()!;

        Console.Clear();
        ExibirCabecalhoFuncionario();
        MostrarLista(funcionarios);

        System.Console.WriteLine("\nInforme o Id do Funcionario");
        string idFuncionario = Console.ReadLine()!;

        if (!OpcaoValida(idFuncionario))
        {
            MostrarMensagemStatus(ConsoleColor.Red, "Opção inválida");
            return null!;
        }

        Funcionario funcionario = (Funcionario)repositorioFuncionario.BuscarPorId(int.Parse(idFuncionario));

        if (!VerificarItemEncontrado(funcionario, "funcionário não localizado"))
            return null!;

        return new Requisicao(medicamento, int.Parse(qtd), paciente, dataRequisicao, crm, funcionario);

    }

    protected override void Visualizar()
    {
        MostrarTexto("-- Selecione uma opção -- ");
        Console.WriteLine("1 - Visualizar todas as requições");
        Console.WriteLine("2 - Visualizar medicamentos mais requeridos");
        string opcao = Console.ReadLine()!;

        if (!OpcaoValida(opcao))
            return;

        else if (opcao == "1")
            TodasAsRequisicoes();

        else if (opcao == "2")
            VisualizarMaisRequeridos();
        else
            MostrarMensagemStatus(ConsoleColor.Red, "Opção Inválida");

    }

    private void TodasAsRequisicoes()
    {
        MostrarTexto("--- Requisições Cadastrados ---");
        ArrayList requisicoes = repositorioRequisicao.BuscarTodos();

        if (!VerificarListaContemItens(requisicoes, "requisições"))
            return;
        ExibirCabecalhoRequisicao();
        RenderizarTabela(requisicoes, true);
    }

    private void VisualizarMaisRequeridos()
    {
        MostrarTexto("--- Medicamentos mais requeridos ---\n");

        ArrayList todasRequisicoes = repositorioRequisicao.BuscarTodos();

        if (!VerificarListaContemItens(todasRequisicoes, "requisições"))
            return;

        var medicamentos = new Dictionary<string, int>();

        foreach (Requisicao requisicao in todasRequisicoes)
        {
            string nomeMedicamento = requisicao.medicamento.nome!;
            int quantidade = 1;

            if (medicamentos.ContainsKey(nomeMedicamento))
                medicamentos[nomeMedicamento] += quantidade;
            else
                medicamentos.Add(nomeMedicamento, quantidade);
        }

        var medicamentosOrdenados = medicamentos.OrderByDescending(x => x.Value);

        foreach (var medicamento in medicamentosOrdenados)
        {
            Console.WriteLine($"{medicamento.Key,-15} | {medicamento.Value}");
        }

        Console.ReadKey();

    }

    private void ExibirCabecalhoRequisicao()
    {
        Console.WriteLine($"{"ID",-3} | {"MEDICAMENTO",-20} {"ID",-3} | {"QTD",-4} | {"PACIENTE",-20} | {"CARTÃO SAÚDE",-12} | {"DATA RECEITA",-15:d} | {"CRM",-8} | {"FUNCIONÁRIO",-15} {"CÓDIGO"}");
    }

    private void ExibirCabecalhoMedicamentos()
    {
        Console.WriteLine($"{"ID",-5} | {"NOME",-20} | {"QTD",-5} | {"FORNECEDOR",-15} | {"FABRICAÇÃO",-12:d} | {"VALIDADE",-12:d} | {"LOTE",-6}");
        Console.WriteLine("------|----------------------|-------|-----------------|--------------|--------------|------------");
    }

    private void ExibirCabecalhoFuncionario()
    {
        Console.WriteLine($"{"ID",-5} | {"NOME",-20} | {"CÓD",-5}");
        Console.WriteLine($"------|-----------------------|------------");
    }
    private void ExibirCabecalhoPaciente()
    {
        Console.WriteLine($"\n{"ID",-3} | {"NOME COMPLETO",-20} | {"IDADE",-5} | {"CARTÃO SAÚDE",-12} | {"TELEFONE",-12} | {"ENDEREÇO"}");
        Console.WriteLine("----|----------------------|-------|--------------|--------------|---------------------------|-------|---------------------------|------------|---------------");
    }

    private void ExibirTabelaMedicamento()
    {
        MostrarTexto("--- Medicamentos Cadastrados ---\n");
        ArrayList medicamentosCadastrados = repositorioMedicamento.BuscarTodos();

        if (!VerificarListaContemItens(medicamentosCadastrados, "medicamentos"))
            return;

        var medicamentos = new List<Medicamento>(medicamentosCadastrados.Cast<Medicamento>());
        var medicamentosOrdenados = medicamentos.OrderBy(m => m.quantidade);

        ExibirCabecalhoMedicamentos();

        foreach (Medicamento item in medicamentosOrdenados)
        {
            if (item.quantidade == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(item);
            }
            else if (item.quantidade < 20)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(item);
            }
            else
                Console.WriteLine(item);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }



}
