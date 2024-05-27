using System.ComponentModel;

namespace SistemaDeTarefas.Enums
{
    public enum StatusTarefa
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Em andamento")]
        EmAndamento = 2,
        [Description("Concluída")]
        Concluida = 3
    }
}
