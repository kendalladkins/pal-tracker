using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PalTracker
{
    [Route("/time-entries")]
    public class TimeEntryController : ControllerBase
    {
        private readonly ITimeEntryRepository Repository;

        private readonly IOperationCounter<TimeEntry> _operationCounter;

        public TimeEntryController(ITimeEntryRepository repository, IOperationCounter<TimeEntry> operationCounter)
        {
            this.Repository = repository;
            this._operationCounter = operationCounter;
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Read(int id)
        {
            _operationCounter.Increment(TrackedOperation.Read);
            return Repository.Contains(id) ? (IActionResult) Ok(Repository.Find(id)) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult Create([FromBody] TimeEntry toCreate)
        {
            _operationCounter.Increment(TrackedOperation.Create);
            var timeEntry = this.Repository.Create(toCreate);
            return new CreatedAtRouteResult("GetTimeEntry", new { id = timeEntry.Id}, timeEntry);
        }

        [HttpGet]
        public ActionResult List()
        {
            _operationCounter.Increment(TrackedOperation.List);
            return new OkObjectResult(this.Repository.List());
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] TimeEntry theUpdate)
        {
            _operationCounter.Increment(TrackedOperation.Update);

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
            _operationCounter.Increment(TrackedOperation.Delete);

            if (this.Repository.Contains(id)) {
                this.Repository.Delete(id);
                return new NoContentResult();
            } else {
                return NotFound();
            }
        }
    }
}