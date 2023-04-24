namespace ControleMedicamentos.ConsoleApp.ModuloEndereco
{
    public class CadastroEndereco
    {
        public Endereco CadastrarEndereco()
        {
            MostrarTexto("--- Cadastro Endereço ---\nInforme a rua:");
            string rua = Console.ReadLine()!;

            MostrarTexto("Informe o número:");
            string nr = Console.ReadLine()!;
            int numero = int.TryParse(nr, out numero) ? int.Parse(nr) : default;

            MostrarTexto("Informe o bairro:");
            string bairro = Console.ReadLine()!;

            MostrarTexto("Informe o cep:");
            string cep = Console.ReadLine()!;

            MostrarTexto("Informe o complemento:");
            string complemento = Console.ReadLine()!;

            return new Endereco(rua, numero, bairro, cep, complemento);

        }

        protected void MostrarTexto(string msg)
        {
            Console.Clear();
            Console.WriteLine(msg);
        }

        public string MostrarCabecalho()
        {
            return ($"{"LOGRADOURO",-25} | {"NRº",-5} | {"BAIRRO",-25} | {"CEP",-10} | {"COMPLEMENTO"}");
        }

    }
}