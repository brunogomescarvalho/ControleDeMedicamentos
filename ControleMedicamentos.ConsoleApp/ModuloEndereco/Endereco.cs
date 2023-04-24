namespace ControleMedicamentos.ConsoleApp.ModuloEndereco
{
    public struct Endereco
    {
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }

        public Endereco(string rua, int numero, string bairro, string cep, string complemento)
        {
            this.Rua = rua;
            this.Numero = numero;
            this.Bairro = bairro;
            this.Cep = cep;
            this.Complemento = complemento;
        }

        public override string ToString()
        {
            return $"{Rua,-25} | {Numero,-5} | {Bairro,-25} | {Cep,-10} | {Complemento}";
        }
    }
}