using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ParveenEvent.Models
{

    public class regmodelcontext : DbContext
    {

        public regmodelcontext() : base("slscon")
        {
        }

        public DbSet<tbl_Reg> regusers { get; set; }

    }
}