using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaDeTarefas.Models;

namespace SistemaDeTarefas.Data.Map
{
    public class UsuarioMap : IEntityTypeConfiguration<UsuarioModel>
    {
        public void Configure(EntityTypeBuilder<UsuarioModel> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            
        }
    }
}
