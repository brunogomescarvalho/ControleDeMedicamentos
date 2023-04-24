using System.Collections;
using System.Globalization;

namespace ControleMedicamentos.ConsoleApp.ModuloCompartilhado
{
    public class Tela
    {
        protected string? OpcaoMenu;
        protected bool Continuar = true;

        public void MostrarMenu(string menu)
        {
            while (Continuar)
            {
                Console.Clear();

                Console.WriteLine($"-- {menu} --");
                Console.WriteLine("1 - Cadastrar");
                Console.WriteLine("2 - Visualizar");
                Console.WriteLine("3 - Editar");
                Console.WriteLine("4 - Excluir");
                Console.WriteLine("9 - Voltar");

                OpcaoMenu = Console.ReadLine()!;

                if (OpcaoValida(OpcaoMenu))
                    switch (OpcaoMenu)
                    {
                        case "1": Cadastrar(); break;
                        case "2": Visualizar(); break;
                        case "3": Editar(); break;
                        case "4": Excluir(); break;
                        case "9": Continuar = false; continue;
                        default: continue;
                    }
            }
        }

        protected virtual void Cadastrar() { }
        protected virtual void Visualizar() { }
        protected virtual void Editar() { }
        protected virtual void Excluir() { }

        protected virtual void RenderizarTabela(ArrayList lista, bool esperarTecla)
        {
            MostrarLista(lista);
            if (esperarTecla)
                Console.ReadKey();
        }

        protected void MostrarMensagemStatus(ConsoleColor cor, string msg)
        {
            Console.Clear();
            Console.ForegroundColor = cor;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        protected void MostrarTexto(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
        }

        protected void MostrarLista(ArrayList lista)
        {
            foreach (var item in lista)
            {
                Console.WriteLine(item);
            }
        }

        protected bool VerificarListaContemItens(ArrayList lista, string item)
        {
            if (lista.Count == 0)
            {
                MostrarMensagemStatus(ConsoleColor.Yellow, $"Lista de {item} sem registros até o momento");
                return false;
            }
            return true;
        }

        protected bool VerificarItemEncontrado(Entidade entidade, string item)
        {
            if (entidade == null)
            {
                MostrarMensagemStatus(ConsoleColor.Red, item);
                return false;
            }
            return true;
        }

        protected bool OpcaoValida(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                foreach (var item in id)
                {
                    if (item < (char)47 || item > (char)57)
                    {
                        MostrarMensagemStatus(ConsoleColor.Red, "Opção Inválida");
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        protected bool ValidarDados(Dictionary<string, object> dados)
        {
            bool dadosValidos = true;
            Console.Clear();

            foreach (var item in dados)
            {
                var prop = item.Value.ToString()!;

                if (prop == string.Empty || prop == "01/01/0001 00:00:00")
                {
                    Console.WriteLine($"* O valor do campo {item.Key} é inválido");
                    dadosValidos = false;
                }
            }

            if (!dadosValidos)
                Console.ReadKey();

            return dadosValidos;
        }

        protected bool ValidarData(string data)
        {
            DateTime dataValida;
            return DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataValida);
        }
    }
}
