using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace LampWebStore.Models
{
    /// <summary>
    /// DB context to interact with DB through EF Core.
    /// </summary>
    public class LampsContext: DbContext
    {
        /// <summary>The lamp entities' repository.</summary>
        public DbSet<Lamp> Lamps { get; set; }


        public LampsContext(DbContextOptions<LampsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Add logging for the DB context to see SQL commands in Debug output
            optionsBuilder.UseLoggerFactory(new LoggerFactory().AddDebug())
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region The initial filling of DB

            int lampId = 0;

            modelBuilder.Entity<Lamp>().HasData(
                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.LED,
                    Manufacturer = "Philips",
                    Cost = 16,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1767083/img_id4205890578895788084.jpeg/9hq"
                },

                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.Fluorescent,
                    Manufacturer = "OSRAM",
                    Cost = 2,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1045304/img_id688239816713013197.jpeg/9hq"
                },

                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.LED,
                    Manufacturer = "OSRAM",
                    Cost = 4.5,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1045304/img_id5760657839941547295.jpeg/9hq"
                },

                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.Fluorescent,
                    Manufacturer = "Camelion",
                    Cost = 2.4,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1045304/img_id4656031640183289008.jpeg/9hq"
                },

                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.LED,
                    Manufacturer = "Gauss",
                    Cost = 2.7,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1045304/img_id4827177099789801154.jpeg/9hq"
                },

                new Lamp {
                    Id = ++lampId,
                    LampType = LampType.Incandescent,
                    Manufacturer = "Philips",
                    Cost = 0.5,
                    ImageRef = "https://avatars.mds.yandex.net/get-mpic/1045304/img_id2657193580316822485.jpeg/9hq"
                }
            );

            #endregion
        }
    }
}
