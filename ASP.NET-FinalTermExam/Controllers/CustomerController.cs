using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASP.NET_FinalTermExam.Controllers
{
    public class CustomerController : Controller
    {
        CustomerService.CustomerService cs = new CustomerService.CustomerService();
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.ContactTitle = cs.GetContactTitle();
            return View();
        }
        public JsonResult SearchResult(CustomerModel.Customer customer)
        {
            var result = cs.GetCustomer(customer);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertCustomer()
        {
            ViewBag.ContactTitle = cs.GetContactTitle();
            return View();
        }
        public ActionResult InsertCustomerAction(CustomerModel.Customer customer) {
            ViewBag.CustomerID=cs.InsertCustomer(customer);
            return Redirect("Index");
        }
    }
}