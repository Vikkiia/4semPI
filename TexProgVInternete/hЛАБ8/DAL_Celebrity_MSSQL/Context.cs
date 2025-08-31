using Microsoft.EntityFrameworkCore;

namespace DAL_Celebrity_MSSQL
{
    public class Context : DbContext   //класс Context, который наследует от DbContext из Entity Framework Core.
    {
        public string? ConnectionString { get; private set; } = null;
        public Context(string connextionString) : base()
        {
            ConnectionString = connextionString;
        }
        /*Присваивает переданную строку подключения в свойство ConnectionString, чтобы её можно было использовать внутри класса — например, при конфигурации подключения.*/

        public Context() : base()
        {

        }

        public DbSet<Celebrity> Celebrities { get; set; }//определяет таблицу Celebrities в контексте базы данных Entity Framework Core.

        public DbSet<LifeEvent> LifeEvents { get; set; }

        // Переопределяем метод OnConfiguring из базового класса DbContext.
        // Этот метод используется для настройки параметров подключения к базе данных.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            // Проверяем, установлена ли строка подключения (например, через конструктор).
            // Если нет — задаём строку подключения по умолчанию.   
            if (this.ConnectionString is null)
            {
                this.ConnectionString = "Server=localhost\\SERVERVILKI; Database=Lab6_Db; Trusted_Connection=true; TrustServerCertificate=True";
            }

            optionsBuilder.UseSqlServer(this.ConnectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Celebrity>().ToTable("Celebrities").HasKey(p => p.Id);   // Указываем, что Id — первичный ключ
            modelBuilder.Entity<Celebrity>().Property(p => p.Id).IsRequired();          // Поле Id обязательно (NOT NULL)
            modelBuilder.Entity<Celebrity>().Property(p => p.FullName).IsRequired().HasMaxLength(50);       // Поле FullName обязательно и ограничено по длине
            modelBuilder.Entity<Celebrity>().Property(p => p.Nationality).IsRequired();
            modelBuilder.Entity<Celebrity>().Property(p => p.ReqPhotoPath).HasMaxLength(200);

            modelBuilder.Entity<LifeEvent>().ToTable("LifeEvents").HasKey(p => p.Id);       // Указываем имя таблицы         // Указываем, что Id — первичный ключ
            modelBuilder.Entity<LifeEvent>().ToTable("LifeEvents");
            modelBuilder.Entity<LifeEvent>().Property(p => p.Id).IsRequired();          // Поле Id обязательно
            modelBuilder.Entity<LifeEvent>().ToTable("LifeEvents").HasOne<Celebrity>().WithMany().HasForeignKey(p => p.CelebrityId);  // Указываем, что LifeEvent связан с Celebrity (один)   // ... и у Celebrity может быть много LifeEvent  // Связь через внешний ключ CelebrityId      
            modelBuilder.Entity<LifeEvent>().Property(p => p.CelebrityId).IsRequired();
            modelBuilder.Entity<LifeEvent>().Property(p => p.Date);              // Свойство Date — дата события (по умолчанию допустимо null)
            modelBuilder.Entity<LifeEvent>().Property(p => p.Description).HasMaxLength(256);
            modelBuilder.Entity<LifeEvent>().Property(p => p.ReqPhotoPath).HasMaxLength(256);


            base.OnModelCreating(modelBuilder);
        }

    }
}
