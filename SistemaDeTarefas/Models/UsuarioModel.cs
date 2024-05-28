using SistemaDeTarefas.Helper;

namespace SistemaDeTarefas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool VerificarSenha(string senha)
        {
            return senha.GerarHash() == Senha;
        }
        public void AlterarSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            //gerar nova senha com guid
            var novaSenha = System.Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }


    }
}
