//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GlazkiSave
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GlazkiSaveEntities : DbContext
    {
        private static GlazkiSaveEntities _context;
        public GlazkiSaveEntities()
            : base("name=GlazkiSaveEntities")
        {
        }
        public static GlazkiSaveEntities GetContext()
        {
            if (_context == null)
                _context = new GlazkiSaveEntities();

            return _context;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agents> Agents { get; set; }
        public virtual DbSet<AgentsLog> AgentsLog { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductSale> ProductSale { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<VW_AgentDetails> VW_AgentDetails { get; set; }
        public virtual DbSet<VW_AgentDetailsDB> VW_AgentDetailsDB { get; set; }
    }
}
