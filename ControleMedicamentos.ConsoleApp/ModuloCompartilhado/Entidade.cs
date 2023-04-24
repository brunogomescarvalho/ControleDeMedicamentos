namespace ControleMedicamentos.ConsoleApp.ModuloCompartilhado;

public class Entidade
{
    public int id { get; protected set; }
    public string? nome { get; protected set; }

    public Entidade(string nome)
    {
        this.nome = nome;
    }

    public Entidade() { }

    public virtual void Editar(Entidade entidade)
    {
        this.nome = entidade.nome;
    }

    public void AtribuirId(int nr)
    {
        this.id = nr;
    }

}