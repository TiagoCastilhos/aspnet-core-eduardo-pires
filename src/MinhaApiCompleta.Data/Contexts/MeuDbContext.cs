using Microsoft.EntityFrameworkCore;
using MinhaApiCompleta.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinhaApiCompleta.Data.Contexts
{
    public class MeuDbContext : DbContext
    {
        public MeuDbContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Fornecedor> Fornecedores { get; set; }
    }
}