using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ticket_Vendor_Machine.Models;

namespace Ticket_Vendor_Machine.Controllers
{
    public class FareStepsController : Controller
    {
        private MetroSystemEntities db = new MetroSystemEntities();

        // GET: FareSteps
        public ActionResult Index()
        {
            return View(db.FareSteps.ToList());
        }

        // GET: FareSteps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FareStep fareStep = db.FareSteps.Find(id);
            if (fareStep == null)
            {
                return HttpNotFound();
            }
            return View(fareStep);
        }

        // GET: FareSteps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FareSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StepID,FromStationCount,ToStationCount,CashPrice,NonCashPrice")] FareStep fareStep)
        {
            if (ModelState.IsValid)
            {
                db.FareSteps.Add(fareStep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fareStep);
        }

        // GET: FareSteps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FareStep fareStep = db.FareSteps.Find(id);
            if (fareStep == null)
            {
                return HttpNotFound();
            }
            return View(fareStep);
        }

        // POST: FareSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StepID,FromStationCount,ToStationCount,CashPrice,NonCashPrice")] FareStep fareStep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fareStep).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fareStep);
        }

        // GET: FareSteps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FareStep fareStep = db.FareSteps.Find(id);
            if (fareStep == null)
            {
                return HttpNotFound();
            }
            return View(fareStep);
        }

        // POST: FareSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FareStep fareStep = db.FareSteps.Find(id);
            db.FareSteps.Remove(fareStep);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
