using System.Web.Mvc;
using DBServer;
using System.Linq;
namespace LinqToSqlite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var entity=new DBContentSev())
            {
                try
                {
                    var query = entity.books.ToList();
                }
                catch (System.Exception ex)
                {

                    var error = ex.ToString();
                }
    
            }
            return View();
        }
    }
}