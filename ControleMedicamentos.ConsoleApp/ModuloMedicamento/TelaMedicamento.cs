using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    public class TelaMedicamento : Tela
    {
        private readonly RepositorioMedicamento repositorioMedicamento;
        private readonly RepositorioFornecedor repositorioFornecedor;

        public TelaMedicamento(RepositorioMedicamento repositorioMedicamento, RepositorioFornecedor repositorioFornecedor)
        {
            this.repositorioFornecedor = repositorioFornecedor;
            this.repositorioMedicamento = repositorioMedicamento;
        }

        protected override void Cadastrar()
        {
            Medicamento medicamento = SolicitarDados();

            if (medicamento == null)
                return;

            if (repositorioMedicamento.ItemCadastrado(medicamento.nome!, medicamento.fornecedor.id))
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Medicamento já cadastrado");
                return;
            }
            repositorioMedicamento.Adicionar(medicamento);
            MostrarMensagemStatus(ConsoleColor.Green, "Medicamento Cadastrado com Sucesso");

        }

        protected override void Visualizar()
        {
            MostrarTexto("--- Medicamentos Cadastrados ---\n");
            ArrayList medicamentosCadastrados = repositorioMedicamento.BuscarTodos();

            if (!VerificarListaContemItens(medicamentosCadastrados, "medicamentos"))
                return;

            var medicamentos = new List<Medicamento>(medicamentosCadastrados.Cast<Medicamento>());
            var medicamentosOrdenados = medicamentos.OrderBy(m => m.quantidade);

            ExibirCabecalhoTabela();

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
                {
                    Console.WriteLine(item);
                }

                Console.ResetColor();
            }

            Console.ReadKey();
        }

        protected override void Editar()
        {
            MostrarTexto("---Editar Medicamento ---\n");

            Console.WriteLine("Informe o id do medicamento");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Opçao inválida");
                return;
            }

            Medicamento medicamento = (Medicamento)repositorioMedicamento.BuscarPorId(int.Parse(id));

            if (!VerificarItemEncontrado(medicamento, "Medicamento não localizado"))
                return;
            do
            {
                MostrarTexto($"\nConfirma editar o medicamento:");

                ExibirCabecalhoTabela();
                Console.WriteLine(medicamento);

                Console.Write("\n\n[1]Sim [2]Não\n\n=>");
                OpcaoMenu = Console.ReadLine()!;
            }
            while (!OpcaoValida(OpcaoMenu));

            if (OpcaoMenu == "2")
                return;

            Medicamento editado = SolicitarDados();

            if (editado == null!)
                return;

            if (repositorioMedicamento.ItemCadastrado(editado.nome!, editado.fornecedor.id))
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Medicamento já cadastrado");
                return;
            }

            medicamento.Editar(editado);

            MostrarMensagemStatus(ConsoleColor.Green, "Medicamento editado com sucesso");
        }

        private Medicamento SolicitarDados()
        {
            Fornecedor fornecedor = BuscarFornecedor();

            if (fornecedor == null)
                return null!;

            MostrarTexto("Informe o nome:");
            string nome = Console.ReadLine()!;

            MostrarTexto("Informe a descrição");
            string descricao = Console.ReadLine()!;

            MostrarTexto("Informe a data de fabricação");
            string data = Console.ReadLine()!;
            DateTime dataFabricacao = ValidarData(data) ? Convert.ToDateTime(data) : default;

            MostrarTexto("Informe a data de vencimento");
            string dataVenc = Console.ReadLine()!;
            DateTime dataVencimento = ValidarData(dataVenc) ? Convert.ToDateTime(dataVenc) : default;

            MostrarTexto("Informe o Lote:");
            string lote = Console.ReadLine()!;

            MostrarTexto("Informe a quantidade:");
            string qtd = Console.ReadLine()!;
            int quantidade = OpcaoValida(qtd) ? int.Parse(qtd) : default;

            Medicamento medicamento = new Medicamento(nome, fornecedor, descricao, dataFabricacao, dataVencimento, lote, quantidade);

            bool dadosValidos = ValidarDados(repositorioMedicamento.ObterValoresPropriedades(medicamento));

            return dadosValidos ? medicamento : null!;
        }

        private Fornecedor BuscarFornecedor()
        {
            ArrayList fornecedores = repositorioFornecedor.BuscarTodos();

            if (VerificarListaContemItens(fornecedores, "fornecedores"))
                RenderizarTabela(fornecedores, false);

            Console.WriteLine("\nInforme o id do fornecedor");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
            {
                MostrarMensagemStatus(ConsoleColor.Red, "Opção inválida");
                return null!;
            }

            Fornecedor fornecedor = (Fornecedor)repositorioFornecedor.BuscarPorId(int.Parse(id));
            if (!VerificarItemEncontrado(fornecedor, "Fornecedor não encontrado"))
                return null!;

            return fornecedor;
        }

        private void ExibirCabecalhoTabela()
        {
            Console.WriteLine($"{"ID",-5} | {"NOME",-20} | {"QTD",-5} | {"FORNECEDOR",-15} | {"FABRICAÇÃO",-12:d} | {"VALIDADE",-12:d} | {"LOTE",-6}");
            Console.WriteLine("------|----------------------|-------|-----------------|--------------|--------------|------------");
        }
    }
}