using System.Collections;
using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;
using ControleMedicamentos.ConsoleApp.ModuloMedicamento;

namespace ControleMedicamentos.ConsoleApp.ModuloAquisicao
{
    public class RepositorioAquisicao : Repositorio
    {

        public bool ItemJaSolicitado(Medicamento medicamento)
        {
            foreach (Aquisicao item in BuscarTodos())
            {
                return item.TodosItensNaLista().Find(i => i.medicamento.id == medicamento.id) != null!;
            }
            return false;
        }


        public bool ItemNaLista(Medicamento medicamento, List<ItemAquisicao> itensCadastrados)
        {
            List<ItemAquisicao> itens = new List<ItemAquisicao>(itensCadastrados);

            return itens.Find(i => i.medicamento.id == medicamento.id) != null;
        }

        public ArrayList BuscarRequisicoesPorStatus(bool status)
        {
            ArrayList aquisicoes = BuscarTodos();
            ArrayList aquisicoesPorStatus = new ArrayList();

            foreach (Aquisicao item in aquisicoes)
            {
                if (item.finalizado == status)
                    aquisicoesPorStatus.Add(item);
            }
            return aquisicoesPorStatus;
        }

    }
}