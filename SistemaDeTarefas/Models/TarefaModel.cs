﻿using SistemaDeTarefas.Enums;

namespace SistemaDeTarefas.Models
{
    public class TarefaModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public StatusTarefa Status{ get; set; }
        public int usuarioID { get; set; }
        public UsuarioModel? Usuario { get; set; }
    }
}
