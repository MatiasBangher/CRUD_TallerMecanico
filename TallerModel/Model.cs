using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
namespace TallerModel
{

    public class TallerContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TallerMecanicoDB");
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Taller;Trusted_Connection=True;");
        }

    }

    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Contraseña { get; set; }
        public string? Email { get; set; }
        public string? Dni { get; set; }
        public string? Cuil { get; set; }
        public string? Telefono { get; set; }
        public Rango? Puesto { get; set; }
    }

    public enum Rango
    {
        Gerente,
        Administrativo,
        DT,
        Mecanico
    }

    public class UsuarioServices
    {
        private TallerContext _context;

        public UsuarioServices()
        {
            _context = new TallerContext();
        }

        public Usuario Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return (usuario);
        }

        public IEnumerable<Usuario> GetAll()
        {
            var usuarios = _context.Usuarios.ToList();
            return usuarios;
        }

        public Usuario? GetById(int id)
        {
            {
                var usuario = _context.Usuarios.SingleOrDefault(u => u.UsuarioId == id);

                if (usuario == null)
                {
                    return null;
                }

                return (usuario);
            }
        }

        public IEnumerable<Usuario>? GetByNombreyAp(string aBuscar)
        {
            var usuarios = _context.Usuarios.Where(u => u.Nombre.Contains(aBuscar) || u.Apellido.Contains(aBuscar)).ToList();

            if (usuarios == null)
            {
                return null;
            }

            return usuarios;
        }

        public IEnumerable<Usuario>? GetByPuesto(Rango puestoABuscar)
        {
            var usuarios = _context.Usuarios.Where(u => u.Puesto == puestoABuscar).ToList();

            if (usuarios == null)
            {
                return null;
            }

            return usuarios;
        }

        public Usuario Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
            return (usuario);
        }

        public int? Delete(int id)
        {
            var usuario = _context.Usuarios.Single(u => u.UsuarioId == id);

            if (usuario == null)
            {
                return null;
            }

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return (id);
        }
    }
    public class Vehiculo
    {
        public required string Patente { get; set; }
        public required string Marca { get; set; }
        public required string Modelo { get; set; }
        public required string Tipo { get; set; } // "electrico", "hibrido", o "naftero"
        public required string Chasis { get; set; }
        public required string Motor { get; set; }
        public int DniApoderado { get; set; }
        public required string NombreApoderado { get; set; }
    }

    public class Turno
    {
        public int TurnoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Vehiculo { get; set; } // Podrías relacionarlo con la clase Vehiculo si lo necesitas
        public string Cliente { get; set; }
        public int? MecanicoId { get; set; } // Puede ser null si no hay mecánico asignado
        public Usuario? Mecanico { get; set; }
    }

    public class TurnoServices
    {
        private List<Turno> turnos;

        public TurnoServices()
        {
            // Inicializar turnos estáticos
            turnos = new List<Turno>
            {
                new Turno { TurnoId = 1, Vehiculo = "Toyota Corolla", Cliente = "Juan Pérez", FechaHora = new DateTime(2024, 11, 24, 10, 0, 0) },
                new Turno { TurnoId = 2, Vehiculo = "Ford Ranger", Cliente = "María López", FechaHora = new DateTime(2024, 11, 24, 11, 0, 0) },
                new Turno { TurnoId = 3, Vehiculo = "Honda Civic", Cliente = "Carlos García", FechaHora = new DateTime(2024, 11, 24, 12, 0, 0) },
                new Turno { TurnoId = 4, Vehiculo = "Chevrolet Onix", Cliente = "Ana Torres", FechaHora = new DateTime(2024, 11, 24, 13, 0, 0) },
                new Turno { TurnoId = 5, Vehiculo = "Volkswagen Golf", Cliente = "Luis Sánchez", FechaHora = new DateTime(2024, 11, 24, 14, 0, 0) }
            };
        }

        public IEnumerable<Turno> GetTurnosSinMecanico()
        {
            return turnos.Where(t => t.MecanicoId == null);
        }

        public void AsignarMecanico(int turnoId, int mecanicoId)
        {
            var turno = turnos.FirstOrDefault(t => t.TurnoId == turnoId);
            if (turno != null)
            {
                turno.MecanicoId = mecanicoId;
            }
            else
            {
                Console.WriteLine("No se encontró el turno.");
            }
        }

        public void DeleteTurno(int turnoId)
        {
            var turno = turnos.FirstOrDefault(t => t.TurnoId == turnoId);
            if (turno != null)
            {
                turnos.Remove(turno);
            }
        }


    }

    public class Mecanico
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    public class MecanicoServices
    {
        private readonly List<Mecanico> mecanicos;

        public MecanicoServices()
        {
            // Datos estáticos simulados
            mecanicos = new List<Mecanico>
            {
                new Mecanico { Id = 1, Nombre = "Carlos", Apellido = "Pérez", Email = "carlos@gmail.com", Telefono = "3624-763498" },
                new Mecanico { Id = 2, Nombre = "Ana", Apellido = "López", Email = "ana@gmail.com", Telefono = "3624-438872" },
                new Mecanico { Id = 3, Nombre = "Luis", Apellido = "García", Email = "luis@gmail.com", Telefono = "3624-349812" }
            };
        }

        public IEnumerable<Mecanico> GetAll()
        {
            return mecanicos;
        }

        public Mecanico? GetById(int id)
        {
            return mecanicos.FirstOrDefault(m => m.Id == id);
        }
    }
}
