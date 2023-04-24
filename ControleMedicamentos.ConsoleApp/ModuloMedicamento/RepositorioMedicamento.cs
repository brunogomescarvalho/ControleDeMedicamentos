using ControleMedicamentos.ConsoleApp.ModuloCompartilhado;

namespace ControleMedicamentos.ConsoleApp.ModuloMedicamento
{
    public class RepositorioMedicamento : Repositorio
    {
        public bool ItemCadastrado(string nome, int id)
        {
            string nomeLower = "";
            foreach (Medicamento item in BuscarTodos())
            {
                foreach (var i in item.nome!)
                    nomeLower += i.ToString().ToLower();

                if (nomeLower == nome.ToLower() && id == item.fornecedor.id)
                    return true;
                nomeLower = "";
            }

            return false;
        }
    }
}