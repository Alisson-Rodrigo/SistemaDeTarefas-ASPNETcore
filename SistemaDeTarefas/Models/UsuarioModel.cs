namespace SistemaDeTarefas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }

        public bool VerificarSenha(string senha)
        {
            return senha.GerarHash() == Senha;
        }
        public void AlterarSenhaHash()
        {
            Senha = Senha.GerarHash();
        }
    }
}
