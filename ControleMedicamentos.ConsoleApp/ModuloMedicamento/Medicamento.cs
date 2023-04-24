using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloFornecedor;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    public class Medicamento : Entidade
    {
        public Fornecedor fornecedor { get; private set; }
        public string descricao { get; private set; }
        public DateTime dataFabricacao { get; private set; }
        public DateTime dataValidade { get; private set; }
        public string nrLote { get; private set; }
        public int quantidade { get; private set; }

        public Medicamento(
            string nome,
            Fornecedor fornecedor,
            string descricao,
            DateTime dataFabricacao,
            DateTime dataValidade,
            string nrLote,
            int quantidade
            ) : base(nome)
        {
            this.fornecedor = fornecedor;
            this.descricao = descricao;
            this.dataFabricacao = dataFabricacao;
            this.dataValidade = dataValidade;
            this.nrLote = nrLote;
            this.quantidade = quantidade;
        }

        public override void Editar(Entidade entidade)
        {
            Medicamento medicamento = (Medicamento)entidade;

            this.fornecedor = medicamento.fornecedor;
            this.descricao = medicamento.descricao;
            this.dataFabricacao = medicamento.dataFabricacao;
            this.dataValidade = medicamento.dataValidade;
            this.nrLote = medicamento.nrLote;
        }

        public void AdicionarQuantidade(int quantidade)
        {
            this.quantidade += quantidade;
        }

        public bool DeduzirQuantidade(int quantidade)
        {
            if (this.quantidade - quantidade > 0)
            {
                this.quantidade -= quantidade;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{id,-5} | {nome,-20} | {quantidade,-5} | {fornecedor.nome,-15} | {dataFabricacao,-12:d} | {dataValidade,-12:d} | {nrLote,-6}";
        }
    }
}