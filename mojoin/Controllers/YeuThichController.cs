using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mojoin.Models;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace mojoin.Controllers
{
    public class YeuThichController : Controller
    {
        DbmojoinContext db = new DbmojoinContext();
        public List<Yeuthich> Layyeuthich()
        {
            List<Yeuthich> lstYeuthich = HttpContext.Session.Get<List<Yeuthich>>("Yeuthich");
            if (lstYeuthich == null)
            {
                lstYeuthich = new List<Yeuthich>();
                HttpContext.Session.Set("Yeuthich", lstYeuthich);
            }
            return lstYeuthich;
        }
        public ActionResult ThemYeuThich(int id, string strURL)
        {
            List<Yeuthich> lstYeuthich = Layyeuthich();
            Yeuthich yt = lstYeuthich.Find(n => n.RoomID == id);
            if (yt == null)
            {
                yt = new Yeuthich(id);
                lstYeuthich.Add(yt);
                return Redirect(strURL);
            }
            else
            {
                lstYeuthich.Remove(yt);
                return Redirect(strURL);
            }
        }
        public ActionResult YeuThich(int id)
        {
            List<Yeuthich> lstGiohang = Layyeuthich();
            var room = db.Rooms
                 .Include(r => r.User).Include(r => r.RoomImages).Where(ri => ri.RoomId == id).ToList()
                 .FirstOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }
            ViewBag.SDT = room.User.Phone;
            ViewBag.Ho = room.User.LastName;
            ViewBag.Ten = room.User.FirstName;
            ViewBag.AVT = room.User.Avatar;
            return View(lstGiohang);
        }
        public ActionResult YeuThichPartial(int id)
        {
            var room = db.Rooms
                 .Include(r => r.User).Include(r => r.RoomImages).Where(ri => ri.RoomId == id).ToList()
                 .FirstOrDefault(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }
            ViewBag.SDT = room.User.Phone;
            ViewBag.Ho = room.User.LastName;
            ViewBag.Ten = room.User.FirstName;
            ViewBag.AVT = room.User.Avatar;
            return PartialView(room);
        }
    }
}
