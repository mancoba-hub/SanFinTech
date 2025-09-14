using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mbiza.Bank
{
    public class MbizaContext : DbContext
    {
        #region Properties

        private readonly ILoggerFactory _loggerFactory;
        public virtual DbSet<Accounts> Account { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MbizaContext"/> class.
        /// </summary>
        /// <param name="contextOptions"></param>
        /// <param name="loggerFactory"></param>
        public MbizaContext(DbContextOptions<MbizaContext> contextOptions, ILoggerFactory loggerFactory) : base(contextOptions)
        {
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MbizaContext"/> class.
        /// </summary>
        public MbizaContext()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Creating the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            modelBuilder.ApplyConfiguration(new AccountsConfiguration());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                .SelectMany(t => t.GetProperties())
                                .Where(p => p.ClrType == typeof(string)))
            {
                if (property.GetMaxLength() == null)
                    property.SetMaxLength(4000);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        #endregion
    }
}
