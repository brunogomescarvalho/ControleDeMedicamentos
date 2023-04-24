using ControleMedicamentos.ConsoleApp.ModuloAquisicao;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;
using ControleMedicamentos.ConsoleApp.ModuloPaciente;
using ControleMedicamentos.ConsoleApp.ModuloRequisicao;

namespace ControleMedicamentos.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        Tela tela = null!;
        bool continuar = true;
        
        RepositorioPaciente repositorioPaciente = new RepositorioPaciente();
        RepositorioFornecedor repositorioFornecedor = new RepositorioFornecedor(); ;
        RepositorioMedicamento repositorioMedicamento = new RepositorioMedicamento();
        RepositorioFuncionario repositorioFuncionario = new RepositorioFuncionario();
        RepositorioRequisicao repositorioRequisicao = new RepositorioRequisicao();
        RepositorioAquisicao repositorioAquisicao = new RepositorioAquisicao();

        AdicionarAlgunsDadosNoSistema(repositorioMedicamento, repositorioPaciente, repositorioFuncionario, repositorioRequisicao, repositorioFornecedor);

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("---- Controle de Medicamentos ----");
            Console.WriteLine("[1] Pacientes");
            Console.WriteLine("[2] Medicamentos");
            Console.WriteLine("[3] Requisições ");
            Console.WriteLine("[4] Fornecedores");
            Console.WriteLine("[5] Funcionários");
            Console.WriteLine("[6] Aquisições");
            Console.WriteLine("[9] Sair");

            Opcoes opcaoMenu = (Opcoes)int.Parse(Console.ReadLine()!);

            switch (opcaoMenu)
            {
                case Opcoes.PACIENTES:
                    tela = new TelaPaciente(repositorioPaciente); break;

                case Opcoes.MEDICAMENTOS:
                    tela = new TelaMedicamento(repositorioMedicamento, repositorioFornecedor); break;

                case Opcoes.REQUISICOES:
                    tela = new TelaRequisicao(repositorioMedicamento, repositorioPaciente, repositorioFuncionario, repositorioRequisicao); break;

                case Opcoes.FORNECEDORES:
                    tela = new TelaFornecedor(repositorioFornecedor); break;

                case Opcoes.FUNCIONARIOS:
                    tela = new TelaFuncionario(repositorioFuncionario); break;

                case Opcoes.AQUISICOES:
                    tela = new TelaAquisicao(repositorioAquisicao, repositorioRequisicao, repositorioMedicamento, repositorioFuncionario); break;

                case Opcoes.SAIR:
                    continuar = false; continue;

                default: continue;
            }

            tela.MostrarMenu($"{opcaoMenu}");
        }
    }

    public enum Opcoes
    {
        PACIENTES = 1,
        MEDICAMENTOS = 2,
        REQUISICOES = 3,
        FORNECEDORES = 4,
        FUNCIONARIOS = 5,
        AQUISICOES = 6,
        SAIR = 9
    }

    private static void AdicionarAlgunsDadosNoSistema
   (
       RepositorioMedicamento repositorioMedicamento,
       RepositorioPaciente repositorioPaciente,
       RepositorioFuncionario repositorioFuncionario,
       RepositorioRequisicao repositorioRequisicao,
       RepositorioFornecedor repositorioFornecedor)
    {
        var medicamentos = repositorioMedicamento.BuscarTodos();
        var pacientes = repositorioPaciente.BuscarTodos();
        var funcionarios = repositorioFuncionario.BuscarTodos();
        var fornecedores = repositorioFornecedor.BuscarTodos();

        repositorioFornecedor.Adicionar(new Fornecedor("Eurofarma", "12345678901", " 11 1234-5678", new ModuloEndereco.Endereco("Av. Vereador J. Diniz", 3146, "Industrial", "88201-111", "São Paulo")));
        repositorioFornecedor.Adicionar(new Fornecedor("EMS", "12345678002", "19 3838-8800", new ModuloEndereco.Endereco("Rod. Jorn F.A Proença", 8, "Chácara Boa Vista", "13186-901", "Hortolândia")));
        repositorioFornecedor.Adicionar(new Fornecedor("Aché", "12345678903", "11 3777-8000", new ModuloEndereco.Endereco("Av. Brigadeiro Faria Lima", 201, "Jardim Paulistano", "01452-000", "São Paulo")));
        repositorioFornecedor.Adicionar(new Fornecedor("Novartis", "12345678904", "11 5532-7122", new ModuloEndereco.Endereco("Rua Francisco L. Vieira", 273, "Parque Ind Anhangüera", "05119-000", "São Paulo")));
        repositorioFornecedor.Adicionar(new Fornecedor("Sanofi", "12345678905", "11 5532-5151", new ModuloEndereco.Endereco("Av. Mutinga", 1805, "Pirituba", "05110-000", "São Paulo")));
        repositorioFornecedor.Adicionar(new Fornecedor("Pfizer", "12345678906", "11 5629-8000", new ModuloEndereco.Endereco("Av. Pres J. Kubitschek", 1838, "Vila Nova Conceição", "04543-000", "São Paulo")));

        repositorioPaciente.Adicionar(new Paciente("Joaci", "Gentil", DateTime.Now.AddYears(-34), "123456", "49 32252340", new ModuloEndereco.Endereco("Paraguai", 146, "Frei Rogério", "88509888", "Casa")));
        repositorioPaciente.Adicionar(new Paciente("Rosemeri", "Gomes", DateTime.Now.AddYears(-63), "987654", "49 32226869", new ModuloEndereco.Endereco("Orli Freitas", 222, "Centro", "88509000", "Ap 202")));
        repositorioPaciente.Adicionar(new Paciente("Nair", "Dos Santos", DateTime.Now.AddYears(-88), "2589", "49 32232274", new ModuloEndereco.Endereco("Pernambuco", 273, "São Cristóvão", "88509120", "Casa Amarela")));

        repositorioFuncionario.Adicionar(new Funcionario("João Silva", "111.222.333-44", "1111-2222", new ModuloEndereco.Endereco("Rua B", 123, "Bairro 3", "3222-5678", "Ap 202")));
        repositorioFuncionario.Adicionar(new Funcionario("Maria Souza", "222.333.444-55", "2222-3333", new ModuloEndereco.Endereco("Rua A", 123, "Bairro 5", "3224-1678", "Casa Madeira")));
        repositorioFuncionario.Adicionar(new Funcionario("Pedro Santos", "333.444.555-66", "3333-4444", new ModuloEndereco.Endereco("Rua D", 123, "Bairro 2", "32266-7898", "CASA")));
        repositorioFuncionario.Adicionar(new Funcionario("Ana Costa", "444.555.666-77", "4444-5555", new ModuloEndereco.Endereco("Rua F", 123, "Bairro 1", "3225-2340", "AP 05")));

        repositorioMedicamento.Adicionar(new Medicamento("Paracetamol", (Fornecedor)fornecedores[0]!, "Analgésico e antitérmico", new DateTime(2023, 4, 1), new DateTime(2025, 3, 31), "123456", 110));
        repositorioMedicamento.Adicionar(new Medicamento("Dipirona", (Fornecedor)fornecedores[0]!, "Analgésico e antitérmico", new DateTime(2023, 4, 2), new DateTime(2025, 4, 1), "123457", 8));
        repositorioMedicamento.Adicionar(new Medicamento("Ibuprofeno", (Fornecedor)fornecedores[1]!, "Analgésico e anti-inflamatório", new DateTime(2023, 4, 3), new DateTime(2025, 5, 2), "123458", 15));
        repositorioMedicamento.Adicionar(new Medicamento("Diclofenaco", (Fornecedor)fornecedores[1]!, "Analgésico e anti-inflamatório", new DateTime(2023, 4, 4), new DateTime(2025, 6, 1), "123459", 25));
        repositorioMedicamento.Adicionar(new Medicamento("Atenolol", (Fornecedor)fornecedores[2]!, "Anti-hipertensivo", new DateTime(2023, 4, 5), new DateTime(2025, 7, 1), "123460", 23));
        repositorioMedicamento.Adicionar(new Medicamento("Losartana", (Fornecedor)fornecedores[2]!, "Anti-hipertensivo", new DateTime(2023, 4, 6), new DateTime(2025, 8, 1), "123461", 11));
        repositorioMedicamento.Adicionar(new Medicamento("Levotiroxina", (Fornecedor)fornecedores[4]!, "Hormônio da tireoide", new DateTime(2023, 4, 9), new DateTime(2025, 11, 1), "123464", 0));
        repositorioMedicamento.Adicionar(new Medicamento("Metformina", (Fornecedor)fornecedores[4]!, "Antidiabético oral", new DateTime(2023, 4, 10), new DateTime(2025, 12, 1), "123465", 0));
        repositorioMedicamento.Adicionar(new Medicamento("Captopril", (Fornecedor)fornecedores[0]!, "Inibidor da enzima conversora de angiotensina", new DateTime(2023, 4, 11), new DateTime(2025, 1, 1), "123466", 100));
        repositorioMedicamento.Adicionar(new Medicamento("AAS", (Fornecedor)fornecedores[0]!, "Anti-inflamatório e antiplaquetário", new DateTime(2023, 4, 12), new DateTime(2025, 2, 1), "123467", 150));
        repositorioMedicamento.Adicionar(new Medicamento("Prednisona", (Fornecedor)fornecedores[1]!, "Corticosteroide", new DateTime(2023, 4, 13), new DateTime(2025, 3, 1), "123468", 200));
        repositorioMedicamento.Adicionar(new Medicamento("Dexametasona", (Fornecedor)fornecedores[1]!, "Corticosteroide", new DateTime(2023, 4, 14), new DateTime(2025, 4, 1), "123469", 250));
        repositorioMedicamento.Adicionar(new Medicamento("Furosemida", (Fornecedor)fornecedores[2]!, "Diurético de alça", new DateTime(2023, 4, 15), new DateTime(2025, 5, 1), "123470", 300));
        repositorioMedicamento.Adicionar(new Medicamento("Hidroclorotiazida", (Fornecedor)fornecedores[2]!, "Diurético tiazídico", new DateTime(2023, 4, 16), new DateTime(2025, 6, 1), "123471", 350));
        repositorioMedicamento.Adicionar(new Medicamento("Sinvastatina", (Fornecedor)fornecedores[3]!, "Inibidor da HMG-CoA redutase", new DateTime(2023, 4, 17), new DateTime(2025, 7, 1), "123472", 400));


        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[8]!, 1, (Paciente)pacientes[0]!, new DateTime(2023, 04, 17), "crm9101", (Funcionario)funcionarios[0]!) as Entidade);
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[6]!, 2, (Paciente)pacientes[0]!, new DateTime(2023, 04, 18), "crm9101", (Funcionario)funcionarios[0]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[4]!, 1, (Paciente)pacientes[0]!, new DateTime(2023, 04, 19), "crm4567", (Funcionario)funcionarios[1]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[1]!, 2, (Paciente)pacientes[0]!, new DateTime(2023, 04, 20), "crm1234", (Funcionario)funcionarios[1]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[8]!, 1, (Paciente)pacientes[1]!, new DateTime(2023, 04, 21), "crm9101", (Funcionario)funcionarios[2]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[2]!, 4, (Paciente)pacientes[2]!, new DateTime(2023, 04, 22), "crm4567", (Funcionario)funcionarios[2]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[3]!, 3, (Paciente)pacientes[2]!, new DateTime(2023, 04, 23), "crm1234", (Funcionario)funcionarios[3]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[5]!, 1, (Paciente)pacientes[2]!, new DateTime(2023, 04, 24), "crm9101", (Funcionario)funcionarios[3]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[1]!, 1, (Paciente)pacientes[2]!, new DateTime(2023, 04, 25), "crm1234", (Funcionario)funcionarios[2]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[0]!, 3, (Paciente)pacientes[1]!, new DateTime(2023, 04, 26), "crm4567", (Funcionario)funcionarios[2]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[7]!, 2, (Paciente)pacientes[0]!, new DateTime(2023, 04, 27), "crm1234", (Funcionario)funcionarios[0]!));
        repositorioRequisicao.Adicionar(new Requisicao((Medicamento)medicamentos[8]!, 4, (Paciente)pacientes[0]!, new DateTime(2023, 04, 28), "crm9101", (Funcionario)funcionarios[0]!));

    }
}