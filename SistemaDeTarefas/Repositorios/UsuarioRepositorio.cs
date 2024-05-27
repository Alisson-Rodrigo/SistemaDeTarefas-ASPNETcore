using SistemaDeTarefas.Data;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using SistemaDeTarefas.Helper;
using System;

namespace SistemaDeTarefas.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly SistemaDeTarefasDbContext _context;

        public UsuarioRepositorio(SistemaDeTarefasDbContext context)
        {
            _context = context;

        }
        public async Task<UsuarioModel> BuscarUsuarioPorId(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UsuarioModel> BuscarUsuarioPorLogin(string login)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<UsuarioModel> BuscarUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<UsuarioModel>> BuscarUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            if (usuario != null)
            {
                var usuarioPorEmail = await BuscarUsuarioPorEmail(usuario.Email);
                if (usuarioPorEmail == null)
                {
                    var usuarioPorLogin = await BuscarUsuarioPorLogin(usuario.Login);
                    if (usuarioPorLogin == null)
                    {
                        usuario.AlterarSenhaHash();
                        await _context.Usuarios.AddAsync(usuario);
                        await _context.SaveChangesAsync();
                        return usuario;
                    }
                    throw new Exception("Login já cadastrado");
                }
                throw new Exception("Email já cadastrado");
            }
            throw new Exception("Erro ao adicionar o usuário");
        }

        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            UsuarioModel usuarioPorId = await BuscarUsuarioPorId(id);
            if (usuarioPorId == null)
            {
                throw new Exception("Erro ao atualizar o usuário");
            }

            usuarioPorId.Nome = usuario.Nome;
            usuarioPorId.Email = usuario.Email;
            usuarioPorId.Login = usuario.Login;
            usuarioPorId.Senha = usuario.Senha;
            _context.Usuarios.Update(usuarioPorId);
            await _context.SaveChangesAsync();
            return usuarioPorId;
        }

        public async Task<bool> Deletar(int id)
        {
            UsuarioModel usuarioPorId = await BuscarUsuarioPorId(id);
            if (usuarioPorId == null)
            {
                throw new Exception("Erro ao deletar o usuário");
            }

            _context.Usuarios.Remove(usuarioPorId);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
