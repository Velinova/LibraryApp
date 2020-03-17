using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library
{
    public partial class db_201920z_va_prj_my_libContext : DbContext
    {
        public db_201920z_va_prj_my_libContext()
        {
        }

        public db_201920z_va_prj_my_libContext(DbContextOptions<db_201920z_va_prj_my_libContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Addedby> Addedby { get; set; }
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Borrowings> Borrowings { get; set; }
        public virtual DbSet<Catalogs> Catalogs { get; set; }
        public virtual DbSet<Containedby> Containedby { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<Librarians> Librarians { get; set; }
        public virtual DbSet<Libraries> Libraries { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Profiles1> Profiles1 { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Reservedby> Reservedby { get; set; }
        public virtual DbSet<Samples> Samples { get; set; }
        public virtual DbSet<Writtenby> Writtenby { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=db_201920z_va_prj_my_lib;Username=db_201920z_va_prj_my_lib_owner;Password=my_lib;Port=5432");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Addedby>(entity =>
            {
                entity.HasKey(e => new { e.Sampleid, e.Profileid })
                    .HasName("pk_sample_librarian_adds");

                entity.ToTable("addedby", "project");

                entity.Property(e => e.Sampleid).HasColumnName("sampleid");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Addedby)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("addedby_profileid_fkey");

                entity.HasOne(d => d.Sample)
                    .WithMany(p => p.Addedby)
                    .HasForeignKey(d => d.Sampleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("addedby_sampleid_fkey");
            });

            modelBuilder.Entity<Authors>(entity =>
            {
                entity.ToTable("authors", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Authorname)
                    .HasColumnName("authorname")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.ToTable("books", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Booklanguage)
                    .HasColumnName("booklanguage")
                    .HasMaxLength(100);

                entity.Property(e => e.Bookname)
                    .IsRequired()
                    .HasColumnName("bookname")
                    .HasMaxLength(100);

                entity.Property(e => e.Genre)
                    .HasColumnName("genre")
                    .HasMaxLength(100);

                entity.Property(e => e.Numberofpages).HasColumnName("numberofpages");

                entity.Property(e => e.Numberofsamples).HasColumnName("numberofsamples");

                entity.Property(e => e.Plot)
                    .HasColumnName("plot")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Borrowings>(entity =>
            {
                entity.ToTable("borrowings", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Borrowdate)
                    .HasColumnName("borrowdate")
                    .HasColumnType("date");

                entity.Property(e => e.Expirationdate)
                    .HasColumnName("expirationdate")
                    .HasColumnType("date");

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Returndate)
                    .HasColumnName("returndate")
                    .HasColumnType("date");

                entity.Property(e => e.Sampleid).HasColumnName("sampleid");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Borrowings)
                    .HasForeignKey(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("borrowings_profileid_fkey");

                entity.HasOne(d => d.Sample)
                    .WithMany(p => p.Borrowings)
                    .HasForeignKey(d => d.Sampleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("borrowings_sampleid_fkey");
            });

            modelBuilder.Entity<Catalogs>(entity =>
            {
                entity.ToTable("catalogs", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Creationdate)
                    .HasColumnName("creationdate")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Containedby>(entity =>
            {
                entity.HasKey(e => new { e.Bookid, e.Catalogid })
                    .HasName("pk_book_catalog_contains");

                entity.ToTable("containedby", "project");

                entity.Property(e => e.Bookid).HasColumnName("bookid");

                entity.Property(e => e.Catalogid).HasColumnName("catalogid");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Containedby)
                    .HasForeignKey(d => d.Bookid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("containedby_bookid_fkey");

                entity.HasOne(d => d.Catalog)
                    .WithMany(p => p.Containedby)
                    .HasForeignKey(d => d.Catalogid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("containedby_catalogid_fkey");
            });

            modelBuilder.Entity<Fees>(entity =>
            {
                entity.ToTable("fees", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Borrowid).HasColumnName("borrowid");

                entity.Property(e => e.Feetotal).HasColumnName("feetotal");

                entity.HasOne(d => d.Borrow)
                    .WithMany(p => p.Fees)
                    .HasForeignKey(d => d.Borrowid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fees_borrowid_fkey");
            });

            modelBuilder.Entity<Librarians>(entity =>
            {
                entity.HasKey(e => e.Profileid)
                    .HasName("librarians_pkey");

                entity.ToTable("librarians", "project");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Profile)
                    .WithOne(p => p.Librarians)
                    .HasForeignKey<Librarians>(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("librarians_profileid_fkey");
            });

            modelBuilder.Entity<Libraries>(entity =>
            {
                entity.ToTable("libraries", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Librarylocation)
                    .HasColumnName("librarylocation")
                    .HasMaxLength(100);

                entity.Property(e => e.Libraryname)
                    .HasColumnName("libraryname")
                    .HasMaxLength(100);

                entity.Property(e => e.Numberofbooks).HasColumnName("numberofbooks");
            });

            modelBuilder.Entity<Members>(entity =>
            {
                entity.HasKey(e => e.Profileid)
                    .HasName("members_pkey");

                entity.ToTable("members", "project");

                entity.Property(e => e.Profileid)
                    .HasColumnName("profileid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Profession)
                    .HasColumnName("profession")
                    .HasMaxLength(100);

                entity.Property(e => e.Registrationdate)
                    .HasColumnName("registrationdate")
                    .HasColumnType("date");

                entity.HasOne(d => d.Profile)
                    .WithOne(p => p.Members)
                    .HasForeignKey<Members>(d => d.Profileid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("members_profileid_fkey");
            });

            modelBuilder.Entity<Profiles>(entity =>
            {
                entity.ToTable("profiles", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Profilename)
                    .HasColumnName("profilename")
                    .HasMaxLength(30);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(30);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(30);

                entity.Property(e => e.Userpassword)
                    .IsRequired()
                    .HasColumnName("userpassword")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Profiles1>(entity =>
            {
                entity.ToTable("profiles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Profilename)
                    .HasColumnName("profilename")
                    .HasMaxLength(30);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(30);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(30);

                entity.Property(e => e.Userpassword)
                    .IsRequired()
                    .HasColumnName("userpassword")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Reservations>(entity =>
            {
                entity.ToTable("reservations", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Profileid).HasColumnName("profileid");

                entity.Property(e => e.Reservationdate)
                    .HasColumnName("reservationdate")
                    .HasColumnType("date");

                entity.Property(e => e.Reservationstatus).HasColumnName("reservationstatus");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Profileid)
                    .HasConstraintName("reservations_profileid_fkey");
            });

            modelBuilder.Entity<Reservedby>(entity =>
            {
                entity.HasKey(e => new { e.Reservationid, e.Bookid })
                    .HasName("pk_book_reservation_reserves");

                entity.ToTable("reservedby", "project");

                entity.Property(e => e.Reservationid).HasColumnName("reservationid");

                entity.Property(e => e.Bookid).HasColumnName("bookid");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Reservedby)
                    .HasForeignKey(d => d.Bookid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reservedby_bookid_fkey");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Reservedby)
                    .HasForeignKey(d => d.Reservationid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reservedby_reservationid_fkey");
            });

            modelBuilder.Entity<Samples>(entity =>
            {
                entity.ToTable("samples", "project");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bookid).HasColumnName("bookid");

                entity.Property(e => e.Libraryid).HasColumnName("libraryid");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Samplestatus).HasColumnName("samplestatus");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Samples)
                    .HasForeignKey(d => d.Bookid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("samples_bookid_fkey");

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.Samples)
                    .HasForeignKey(d => d.Libraryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("samples_libraryid_fkey");
            });

            modelBuilder.Entity<Writtenby>(entity =>
            {
                entity.HasKey(e => new { e.Bookid, e.Authorid })
                    .HasName("pk_book_author_written");

                entity.ToTable("writtenby", "project");

                entity.Property(e => e.Bookid).HasColumnName("bookid");

                entity.Property(e => e.Authorid).HasColumnName("authorid");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Writtenby)
                    .HasForeignKey(d => d.Authorid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("writtenby_authorid_fkey");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Writtenby)
                    .HasForeignKey(d => d.Bookid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("writtenby_bookid_fkey");
            });
        }
    }
}
