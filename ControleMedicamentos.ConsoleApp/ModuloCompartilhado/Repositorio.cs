using System.Collections;
using System.Reflection;
namespace ControleMedicamentos.ConsoleApp.ModuloCompartilhado
{
    public class Repositorio
    {
        protected ArrayList registros = new ArrayList();
        private int contadorId = 1;

        public void Adicionar(Entidade entidade)
        {
            entidade.AtribuirId(contadorId++);
            this.registros.Add(entidade);
        }

        public void Remover(Entidade entidade)
        {
            this.registros.Remove(entidade);
        }

        public ArrayList BuscarTodos()
        {
            return registros;
        }

        public Entidade BuscarPorId(int id)
        {
            foreach (Entidade item in registros)
            {
                if (item.id == id)
                    return item;
            }
            return null!;
        }

        public bool ValidarCampos(ArrayList obj)
        {
            foreach (var item in obj)
            {
                if (item == null || item == default)
                    return false;
            }
            return true;
        }

        public Dictionary<string, object> ObterValoresPropriedades(Object obj)
        {
            var valorPropriedades = new Dictionary<string, object>();

            Type objeto = obj.GetType();

            PropertyInfo[] propriedades = objeto.GetProperties();

            foreach (var item in propriedades)
            {
                valorPropriedades.Add(item.Name, item.GetValue(obj, null)!);
            }

            return valorPropriedades;
        }
    }
}