using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using SistemaDeTarefas.Helper.Interfaces;
using SistemaDeTarefas.Models;
namespace SistemaDeTarefas.Helper
{
    public class EnviarEmailRecuperacao : IEnviarEmailRecuperacao
    {
        private readonly IConfiguration _configuration;

        public EnviarEmailRecuperacao(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string email, string assunto, string mensagem)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string username = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, nome)
                };

                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.Credentials = new NetworkCredential(username, senha);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                    Console.WriteLine($"{host}, {nome}, {username}, {senha}, {porta}");
                    Console.WriteLine(email);
                    return true;
                }
            }
            catch (System.Exception)
            {
                // Gravar log de erro ao enviar e-mail

                return false;
            }
        }
    }
}
