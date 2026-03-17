
using Business.Services;
using Core.Abstracts.IServices;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService service;
        public PostsController()
        {
            service = new PostService();
        }
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null) 
                return HttpNotFound();
            var post = service.GetPostDetail((int)id);
            if (post== null)
                return HttpNotFound();
            return View(post);
        }
    }
}