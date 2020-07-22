using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest.Data
{
    public class ConversationDataContext : DbContext
    {
        public ConversationDataContext()
            : base("ConversationDataContextConnectionString")
        {
        }
        public DbSet<Activity> Activities { get; set; }

    }
}
