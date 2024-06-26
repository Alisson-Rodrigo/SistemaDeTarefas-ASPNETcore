﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using SistemaDeTarefas.Models;
using SistemaDeTarefas.Repositorios.Interfaces;

namespace SistemaDeTarefas.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioModel>>> BuscarTodosUsuarios()
        {
            List<UsuarioModel> usuarios = await _usuarioRepositorio.BuscarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioModel>> BuscarUserID(int id)
        {
            UsuarioModel usuarios = await _usuarioRepositorio.BuscarUsuarioPorId(id);
            return Ok(usuarios);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UsuarioModel>> AtualizarUsuario(int id, [FromBody] UsuarioModel usuario)
        {
            try
            {
                UsuarioModel usuarioAtualizado = await _usuarioRepositorio.Atualizar(usuario, id);
                return Ok(usuarioAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletarUsuario(int id)
        {
            bool usuarioDeletado = await _usuarioRepositorio.Deletar(id);
            return Ok(usuarioDeletado);
        }


    }
}
