using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloFornecedor;

public class TelaFornecedor : Tela
{
    RepositorioFornecedor repositorioFornecedor;
    CadastroEndereco cadastroEndereco;

    public TelaFornecedor(RepositorioFornecedor repositorioFornecedor)
    {
        this.repositorioFornecedor = repositorioFornecedor;
        this.cadastroEndereco = new CadastroEndereco();
    }

    private Fornecedor SolicitarDados()
    {
        MostrarTexto("Informe o nome do fornecedor:");
        string nome = Console.ReadLine()!;

        MostrarTexto("Informe o CNPJ:");
        string cnpj = Console.ReadLine()!;

        MostrarTexto("Telefone:");
        string telefone = Console.ReadLine()!;

        Endereco endereco = cadastroEndereco.CadastrarEndereco();
        Fornecedor fornecedor = new Fornecedor(nome, cnpj, telefone, endereco);
        
        bool fornecedorValido = ValidarDados(repositorioFornecedor.ObterValoresPropriedades(fornecedor));
        bool enderecoValido = ValidarDados(repositorioFornecedor.ObterValoresPropriedades(endereco));

        return fornecedorValido && enderecoValido ? fornecedor : null!;

    }

    protected override void Editar()
    {
        MostrarTexto("--- Editar Fornecedores ---");
        ArrayList fornecedores = repositorioFornecedor.BuscarTodos();

        if (VerificarListaContemItens(fornecedores, "fornecedores"))
            RenderizarTabela(fornecedores, false);

        Console.WriteLine("\nInforme o id do fornecedor");
        string id = Console.ReadLine()!;

        if (!OpcaoValida(id))
        {
            MostrarMensagemStatus(ConsoleColor.Red, "Opção inválida");
            return;
        }

        Fornecedor fornecedor = (Fornecedor)repositorioFornecedor.BuscarPorId(int.Parse(id));

        if (VerificarItemEncontrado(fornecedor, "Fornecedor não localizado"))
            return;

        Fornecedor atualizado = SolicitarDados();

        if (atualizado == null)
            return;

        fornecedor.Editar(atualizado);

        MostrarMensagemStatus(ConsoleColor.Green, "Cadastro editado com sucesso");

    }

    protected override void Cadastrar()
    {
        Fornecedor fornecedor = SolicitarDados();

        if (fornecedor == null)
            return;
        this.repositorioFornecedor.Adicionar(fornecedor);
        MostrarMensagemStatus(ConsoleColor.Green, "Fornecedor cadastrado com sucesso!");
    }

    protected override void Visualizar()
    {
        MostrarTexto("--- Fornecedores Cadastrados ---");
        ArrayList fornecedores = repositorioFornecedor.BuscarTodos();

        if (VerificarListaContemItens(fornecedores, "fornecedores"))
            RenderizarTabela(fornecedores, true);
    }

    protected override void RenderizarTabela(ArrayList fornecedores, bool esperarTecla)
    {
        ExibirCabecalhoTabela();
        base.RenderizarTabela(fornecedores, esperarTecla);
    }

    private void ExibirCabecalhoTabela()
    {
        Console.WriteLine($"\n{"ID",-3} | {"NOME",-15} | {"CNPJ",-15} | {"TELEFONE",-15} | {cadastroEndereco.MostrarCabecalho()}");
        Console.WriteLine("----|-----------------|-----------------|-----------------|---------------------------|-------|---------------------------|------------|---------------");
    }

}

