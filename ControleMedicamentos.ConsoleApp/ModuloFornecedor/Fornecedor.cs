using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloEndereco;

namespace ControleMedicamentos.ConsoleApp.ModuloFornecedor
{
    public class Fornecedor : Entidade
    {
        public string cnpj { get; set; }
        public string telefone { get; set; }
        public Endereco endereco { get; set; }
        
        public Fornecedor(string nome, string cnpj, string telefone, Endereco endereco) : base(nome)
        {
            this.cnpj = cnpj;
            this.telefone = telefone;
            this.endereco = endereco;
        }

        public override void Editar(Entidade entidade)
        {
            Fornecedor fornecedor = (Fornecedor)entidade;
            
            this.nome = fornecedor.nome;
            this.cnpj = fornecedor.cnpj;
            this.telefone = fornecedor.telefone;
            this.endereco = fornecedor.endereco;
        }

        public override string ToString()
        {
            return $"{id,-3} | {nome,-15} | {cnpj,-15} | {telefone,-15} | {endereco}";
        }
    }
}