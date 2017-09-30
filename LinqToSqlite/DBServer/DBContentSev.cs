using System.Data.Entity;

namespace DBServer
{
    public class DBContentSev: DbContext
    {
        public DBContentSev() : base("DBContentSev")
        {
            this.Configuration.LazyLoadingEnabled = false;//禁用延时加载
        }
        /// <summary>
        /// 表映射
        /// </summary>
        public DbSet<Peoples> peoples { get; set; }
        /// <summary>
        /// 表映射
        /// </summary>
        public DbSet<Books> books { get; set; }
    }
}
