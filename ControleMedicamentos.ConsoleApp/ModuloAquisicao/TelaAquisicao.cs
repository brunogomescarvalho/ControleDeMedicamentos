using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using System.Collections;

namespace ControleMedicamentos.ConsoleApp.ModuloAquisicao
{
    public class TelaAquisicao : Tela
    {
        private readonly RepositorioAquisicao repositorioAquisicao;
        private readonly RepositorioRequisicao repositorioRequisicao;
        private readonly RepositorioMedicamento repositorioMedicamento;
        private readonly RepositorioFuncionario repositorioFuncionario;

        public TelaAquisicao(RepositorioAquisicao repositorioAquisicao, RepositorioRequisicao repositorioRequisicao, RepositorioMedicamento repositorioMedicamento, RepositorioFuncionario repositorioFuncionario)
        {
            this.repositorioAquisicao = repositorioAquisicao;
            this.repositorioRequisicao = repositorioRequisicao;
            this.repositorioMedicamento = repositorioMedicamento;
            this.repositorioFuncionario = repositorioFuncionario;
        }

        protected override void Cadastrar()
        {
            while (true)
            {
                MostrarTexto("-- Cadastrar --");
                Console.WriteLine("1 - Nova Aquisição");
                Console.WriteLine("2 - Recebimento Pedido");
                Console.WriteLine("3 - Voltar");
                OpcaoMenu = Console.ReadLine();

                if (OpcaoValida(OpcaoMenu!))
                    switch (OpcaoMenu)
                    {
                        case "1": EfetuarAquisicao(); break;
                        case "2": RegistrarRecebimentoPedido(); break;
                        case "3": return;
                        default: continue;
                    }
            }
        }

        protected override void Visualizar()
        {
            while (true)
            {
                MostrarTexto("-- Visualizar --");
                Console.WriteLine("1 - Todas As Aquisições");
                Console.WriteLine("2 - Aquisições em Aberto");
                Console.WriteLine("3 - Aquisições Finalizadas");
                Console.WriteLine("4 - Voltar");
                OpcaoMenu = Console.ReadLine();

                if (OpcaoValida(OpcaoMenu!))
                    switch (OpcaoMenu)
                    {
                        case "1": TodasAsAquisicoes(); break;
                        case "2": AquisicoesEmAberto(); break;
                        case "3": AquisicoesFinalizadas(); break;
                        case "4": return;
                        default: continue;
                    }
            }

        }

        private void RegistrarRecebimentoPedido()
        {
            MostrarTexto(" --- Registrar Chegada Pedido ---");
            ArrayList aquisicoesEmAberto = repositorioAquisicao.BuscarRequisicoesPorStatus(false);

            if (!VerificarListaContemItens(aquisicoesEmAberto, "aquisicões em aberto"))
                return;

            ExibirCabecalhoAquisicao();
            MostrarLista(aquisicoesEmAberto);

            Console.WriteLine("\nDigite o Id da Solicitação Para Atualizar o Estoque");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
                return;

            Aquisicao aquisicao = (Aquisicao)repositorioAquisicao.BuscarPorId(int.Parse(id));

            if (!VerificarItemEncontrado(aquisicao, "Id Não Localizado"))
            {
                return;
            }

            else if (aquisicao.finalizado == true)
            {
                MostrarMensagemStatus(ConsoleColor.Yellow, "Aquisição ja finalizada");
                return;
            }

            foreach (var item in aquisicao.TodosItensNaLista())
            {
                item.medicamento.AdicionarQuantidade(item.quantidade);
            }

            aquisicao.Finalizar();

            MostrarMensagemStatus(ConsoleColor.Green, "Estoque atualizado com sucesso");

        }



        private void EfetuarAquisicao()
        {
            MostrarTexto("--- Efetuar Solicitação ---\n");

            ArrayList funcionarios = repositorioFuncionario.BuscarTodos();

            if (!VerificarListaContemItens(funcionarios, "funcionários"))
                return;

            ExibirCabecalho();
            RenderizarTabela(funcionarios, false);

            Aquisicao aquisicao = InicializarAquisicao();

            Console.Clear();

            AdicionarMedicamentosNaLista(aquisicao);
            repositorioAquisicao.Adicionar(aquisicao);
            MostrarMensagemStatus(ConsoleColor.Green, "Solicitação Efetuada com Sucesso");

        }

        protected override void Editar()
        {
            MostrarTexto("--- Editar Solicitação ---\n");

            ArrayList solicitacoes = repositorioAquisicao.BuscarTodos();

            if (!VerificarListaContemItens(solicitacoes, "aquisições"))
                return;

            ExibirCabecalhoAquisicao();
            MostrarLista(solicitacoes);

            Console.WriteLine("\n\nDigite o Id da Solicitação Editar");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
                return;

            Aquisicao aquisicao = (Aquisicao)repositorioAquisicao.BuscarPorId(int.Parse(id));

            if (!VerificarItemEncontrado(aquisicao, "Id Não Localizado..."))
                return;

            ArrayList funcionarios = repositorioFuncionario.BuscarTodos();

            if (!VerificarListaContemItens(funcionarios, "funcionários"))
                return;

            ExibirCabecalho();
            RenderizarTabela(funcionarios, false);

            aquisicao.LimparLista();

            Aquisicao aquisicaoEditada = InicializarAquisicao();
            AdicionarMedicamentosNaLista(aquisicaoEditada);
            aquisicao.Editar(aquisicaoEditada);

            MostrarMensagemStatus(ConsoleColor.Green, "Solicitação Editada com Sucesso");

        }

        private void AdicionarMedicamentosNaLista(Aquisicao aquisicao)
        {
            while (true)
            {
                MostrarMedicamentos();

                Console.WriteLine("\n\nInforme o id do Medicamento ou Tecle Enter para Finalizar");
                string id = Console.ReadLine()!;

                if (string.IsNullOrEmpty(id))
                {
                    if (ConfirmarFinalizar(aquisicao))
                    {
                        return;
                    }

                    continue;
                }

                if (!OpcaoValida(id))
                    continue;

                Medicamento medicamento = (Medicamento)repositorioMedicamento.BuscarPorId(Convert.ToInt32(id));

                if (!VerificarItemEncontrado(medicamento, "Id Não Encontrado"))
                    continue;

                if (medicamento.quantidade > 20)
                {
                    MostrarMensagemStatus(ConsoleColor.Yellow, "Medicamento com estoque suficiente");
                    continue;
                }

                if (repositorioAquisicao.ItemJaSolicitado(medicamento))
                {
                    MostrarMensagemStatus(ConsoleColor.Yellow, "Medicamento já incluído em outra solicitação");
                    continue;
                }

                List<ItemAquisicao> itensCadastrados = aquisicao.TodosItensNaLista();

                bool jaIncluido = repositorioAquisicao.ItemNaLista(medicamento, itensCadastrados);

                if (jaIncluido)
                    do
                    {
                        MostrarTexto("O Medicamento Informado Já Está na Lista, Deseja Continuar? [1]Sim [2]Não");
                        OpcaoMenu = Console.ReadLine()!;
                    }
                    while (!OpcaoValida(OpcaoMenu) && OpcaoMenu != "1" && OpcaoMenu != "2");

                if (OpcaoMenu == "2")
                    continue;

                int qtd = SolicitarQuantidade();

                if (jaIncluido)
                {
                    ItemAquisicao med = itensCadastrados.Find(i => i.medicamento.id == medicamento.id)!;
                    med!.IncluirQuantidade(qtd);
                }
                else
                {
                    ItemAquisicao novoItem = new ItemAquisicao(medicamento, qtd);
                    aquisicao.AdicionarItem(novoItem);
                }

                do
                {
                    MostrarTexto("Deseja Incluir Mais Algum Item ? [1]Sim [2]Não");
                    OpcaoMenu = Console.ReadLine()!;
                }
                while (!OpcaoValida(OpcaoMenu) && OpcaoMenu != "1" && OpcaoMenu != "2");

                if (OpcaoMenu == "2")
                    break;

            }
        }

        private Aquisicao InicializarAquisicao()
        {
            Console.WriteLine("\nInforme o Id do Funcionário");
            string idfuncionario = Console.ReadLine()!;

            if (!OpcaoValida(idfuncionario))
                return null!;

            Funcionario funcionario = (Funcionario)repositorioFuncionario.BuscarPorId(Convert.ToInt32(idfuncionario));

            if (!VerificarItemEncontrado(funcionario, "Funcionário Não Encontrado"))
                return null!;

            return new Aquisicao(funcionario);
        }

        private int SolicitarQuantidade()
        {
            string qtd = "";
            bool ehValido = true;

            do
            {
                MostrarTexto("\nInforme a Quantidade");
                qtd = Console.ReadLine()!;
                ehValido = OpcaoValida(qtd);

                if (ehValido)
                {
                    if (int.Parse(qtd) <= 0)
                    {
                        MostrarMensagemStatus(ConsoleColor.Red, "A Quantidade Precisa Ser Maior Que Zero");
                        ehValido = false;
                    }
                }
            } while (!ehValido);

            return int.Parse(qtd);
        }

        private bool ConfirmarFinalizar(Aquisicao aquisicao)
        {
            do
            {
                MostrarTexto("Confirmar Finalizar o Pedido? [1] Sim [2] Não");
                OpcaoMenu = Console.ReadLine()!;
            }
            while (!OpcaoValida(OpcaoMenu));

            if (OpcaoMenu == "1")
            {
                return true;
            }
            if (aquisicao.TodosItensNaLista().Count == 0)
            {
                return true;
            }
            return false;
        }


        private void AquisicoesEmAberto()
        {
            MostrarTexto("--- Aquisições Em Aberto ---");
            ArrayList solicitacoes = repositorioAquisicao.BuscarRequisicoesPorStatus(false);

            if (!VerificarListaContemItens(solicitacoes, "aquisições"))
                return;

            RenderizarTabela(solicitacoes);

        }

        private void AquisicoesFinalizadas()
        {
            MostrarTexto("--- Aquisições Finalizadas ---");
            ArrayList solicitacoes = repositorioAquisicao.BuscarRequisicoesPorStatus(true);

            if (!VerificarListaContemItens(solicitacoes, "aquisições"))
                return;
            RenderizarTabela(solicitacoes);

        }

        private void TodasAsAquisicoes()
        {
            MostrarTexto("--- Aquisições Cadastradas ---");
            ArrayList solicitacoes = repositorioAquisicao.BuscarTodos();

            if (!VerificarListaContemItens(solicitacoes, "aquisições"))
                return;
            RenderizarTabela(solicitacoes);
        }

        private void DetalhesAquisicao()
        {
            Console.WriteLine("\n\nDigite o Id da Solicitação Para Ver Os Detalhes Ou Tecle Enter Para Voltar");
            string id = Console.ReadLine()!;

            if (!OpcaoValida(id))
                return;

            Aquisicao aquisicao = (Aquisicao)repositorioAquisicao.BuscarPorId(int.Parse(id));

            if (!VerificarItemEncontrado(aquisicao, "Id Não Localizado..."))
                return;

            Console.Clear();

            Console.WriteLine("-- Detalhes Aquisição --");
            ExibirCabecalhoAquisicao();
            Console.WriteLine($"{aquisicao}");

            Console.WriteLine("\n --- Itens --- \n");
            Console.WriteLine($"{"MEDICAMENTO",-20} | {"ID",-5} | {"QUANTIDADE"}");
            Console.WriteLine($"---------------------|-------|------------");

            foreach (ItemAquisicao item in aquisicao.TodosItensNaLista())
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }

        private void MostrarMedicamentos()
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

        protected override void Excluir()
        {
            Console.Clear();
            ArrayList solicitacoes = repositorioAquisicao.BuscarTodos();

            if (!VerificarListaContemItens(solicitacoes, "aquisições"))
                return;

            ExibirCabecalhoAquisicao();
            MostrarLista(solicitacoes);

            Console.WriteLine("\n\nDigite o Id da Solicitação Editar");
            string id = Console.ReadLine()!;

            Aquisicao aquisicao = (Aquisicao)repositorioAquisicao.BuscarPorId(int.Parse(id));

            if (!VerificarItemEncontrado(aquisicao, "Id Não Localizado..."))
                return;

            if (aquisicao.finalizado == true)
            {
                MostrarMensagemStatus(ConsoleColor.Yellow, "Não é possivél excluir solicitações finalizadas");
                return;
            }

            repositorioAquisicao.Remover(aquisicao);
            MostrarMensagemStatus(ConsoleColor.Green, "Solicitação excluída");

        }

        private void RenderizarTabela(ArrayList solicitacoes)
        {
            ExibirCabecalhoAquisicao();
            MostrarLista(solicitacoes);
            DetalhesAquisicao();
        }
        private void ExibirCabecalhoMedicamentos()
        {
            Console.WriteLine($"{"ID",-5} | {"NOME",-20} | {"QTD",-5} | {"FORNECEDOR",-15} | {"FABRICAÇÃO",-12:d} | {"VALIDADE",-12:d} | {"LOTE",-6}");
            Console.WriteLine("------|----------------------|-------|-----------------|--------------|--------------|------------");
        }

        private void ExibirCabecalhoAquisicao()
        {
            Console.WriteLine($"\n{"ID",-5} | {"FUNCIONÁRIO",-20} {"CÓDIGO",8} | {"DATA SOLICITAÇÃO",-18} | {"ENTREGUE",-10} | {"DATA RECEBIMENTO"}");
            Console.WriteLine("------|-------------------------------|--------------------|------------|--------------------");
        }

        private void ExibirCabecalho()
        {
            Console.WriteLine($"{"ID",-5} | {"NOME",-20} | {"CÓD",-5}");
            Console.WriteLine($"------|----------------------|------------");
        }
    }

}
