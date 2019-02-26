using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        readonly ITimeEntryRepository Repository;

        public TimeEntryController(ITimeEntryRepository repository)
        {
            this.Repository = repository;
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Read(int id)
        {
            return Repository.Contains(id) ? (IActionResult) Ok(Repository.Find(id)) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Create([FromBody] TimeEntry toCreate)
        {
            var timeEntry = this.Repository.Create(toCreate);
            return new CreatedAtRouteResult("GetTimeEntry", new { id = timeEntry.Id}, timeEntry);
        }

        [HttpGet]
        public ActionResult List()
        {
            return new OkObjectResult(this.Repository.List());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] TimeEntry theUpdate)
        {
            if (this.Repository.Contains(id)) {
                var timeEntry = this.Repository.Update(id, theUpdate);
                return new OkObjectResult(timeEntry);
            } else {
                return NotFound();
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            if (this.Repository.Contains(id)) {
                this.Repository.Delete(id);
                return new NoContentResult();
            } else {
                return NotFound();
            }
        }
    }
}