using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFuncionario;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;

namespace ControleMedicamentos.ConsoleApp.ModuloAquisicao
{
    public class Aquisicao : Entidade
    {
        public DateTime dataSolicitacao { get; private set; }
        public DateTime? dataRecebimento { get; private set; }
        public bool finalizado { get => dataRecebimento != null; }
        public Funcionario funcionario { get; private set; }
        private List<ItemAquisicao> itensAquisicao;

        public Aquisicao(Funcionario funcionario)
        {
            this.dataSolicitacao = DateTime.Now;
            this.funcionario = funcionario;
            this.itensAquisicao = new List<ItemAquisicao>();
        }

        public override void Editar(Entidade entidade)
        {
            Aquisicao aquisicao = (Aquisicao)entidade;
            this.dataSolicitacao = DateTime.Now;
            this.funcionario = aquisicao.funcionario;
            this.itensAquisicao = aquisicao.itensAquisicao;
        }



        public void AdicionarItem(ItemAquisicao item)
        {
            this.itensAquisicao.Add(item);
        }

        public void Finalizar()
        {
            this.dataRecebimento = DateTime.Now;
        }

        public List<ItemAquisicao> TodosItensNaLista()
        {
            return itensAquisicao;
        }

        public void LimparLista()
        {
            this.itensAquisicao = new List<ItemAquisicao>();
        }

        public override string ToString()
        {
            return $"{id,-5} | {funcionario.nome,-20} {funcionario.codigo,8} | {dataSolicitacao,-18:d} | {(finalizado ? "Sim" : "Não"),-10} | {dataRecebimento,-18:d}";
        }

    }


    public class ItemAquisicao
    {
        public Medicamento medicamento { get; private set; }
        public int quantidade { get; private set; }

        public ItemAquisicao(Medicamento medicamento, int quantidade)
        {
            this.medicamento = medicamento;
            this.quantidade = quantidade;
        }

        public void IncluirQuantidade(int qtd)
        {
            this.quantidade += qtd;
        }

        public override string ToString()
        {
            return $"{medicamento.nome,-20} | {medicamento.id,-5} | {quantidade}";
        }
    }
}