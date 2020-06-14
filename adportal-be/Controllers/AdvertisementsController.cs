﻿using adportal_be.Data;
using adportal_be.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace adportal_be.Controllers
{
    public class AdvertisementsController : ApiController
    {

        AdPortalDbContext adportalDbContext = new AdPortalDbContext();

        // GET: api/Advertisement
        [Route("api/Advertisements/Category/{CategoryId}")]
        public IHttpActionResult GetAdvertisementsByCategory(int CategoryId)
        {
            var advertisements = new List<Advertisement>();
            using (var ctx = new AdPortalDbContext())
            {
                advertisements = ctx.Advertisements.SqlQuery(
                    "SELECT * FROM Advertisements where Category_Id=@CategoryId", new System.Data.SqlClient.SqlParameter("@CategoryId", CategoryId)
                    ).ToList<Advertisement>();
            }
            return Ok(advertisements);
        }

        [Route("api/Advertisements/Type/{TypeId}")]
        public IHttpActionResult GetAdvertisementByType (int TypeId)
        {
            var advertisements = new List<Advertisement>();
            using (var ctx = new AdPortalDbContext())
            {
                advertisements = ctx.Advertisements.SqlQuery(
                    "SELECT * FROM Advertisements where AdType_Id=@TypeId", new System.Data.SqlClient.SqlParameter("@TypeId", TypeId)
                    ).ToList<Advertisement>();
            }
            return Ok(advertisements);
        }


        public IHttpActionResult Get(string sort)
        {
            IQueryable<Advertisement> advertisements;
            switch (sort)
            {
                case "newer":
                    advertisements = adportalDbContext.Advertisements.OrderByDescending(a => a.CreationDate);
                    break;
                case "older":
                    advertisements = adportalDbContext.Advertisements.OrderBy(a => a.CreationDate);
                    break;
                default:
                    advertisements = adportalDbContext.Advertisements;
                    break;
            }
            
            return Ok(advertisements);
        }

        // GET: api/Advertisement/5
        public IHttpActionResult Get(int id)
        {
            var advertisement = adportalDbContext.Advertisements.Find(id);
            if (advertisement == null)
            {
                return BadRequest("No advertisement with such id");
            }
            return Ok(advertisement);
        }

        // POST: api/Advertisement
        public IHttpActionResult Post([FromBody] Advertisement advertisement)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            advertisement.CreationDate = DateTime.Now;
            adportalDbContext.Advertisements.Add(advertisement);
            adportalDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Advertisement/5
        public IHttpActionResult Put(int id, [FromBody] Advertisement advertisement)
        {
            var entity = adportalDbContext.Advertisements.FirstOrDefault(a => a.Id == id);
            if(entity == null)
            {
                return BadRequest("No advertisement with such id found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            entity.Title = advertisement.Title;
            entity.Description = advertisement.Description;
            entity.Price = advertisement.Price;
            entity.Active = advertisement.Active;
            return Ok(entity);
        }

        // DELETE: api/Advertisement/5
        public IHttpActionResult Delete(int id)
        {
            var advertisement = adportalDbContext.Advertisements.Find(id);
            if (advertisement == null)
            {
                return BadRequest("No advertisement with such id found");
            }
            adportalDbContext.Advertisements.Remove(advertisement);
            adportalDbContext.SaveChanges();
            return Ok("Advertisement deleted!");
        }
    }
}
