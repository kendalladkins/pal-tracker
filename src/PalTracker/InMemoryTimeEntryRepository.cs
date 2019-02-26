using System.Collections.Generic;

namespace PalTracker
{
    public class InMemoryTimeEntryRepository : ITimeEntryRepository
    {

        private long IdCounter = 0;
        Dictionary<long, TimeEntry> Dictionary = new Dictionary<long, TimeEntry>();

        public bool Contains(long id)
        {
            return Dictionary.ContainsKey(id);
        }

        public TimeEntry Create(TimeEntry timeEntry)
        {
            timeEntry.Id = ++IdCounter;
            Dictionary.Add(IdCounter, timeEntry);
            return timeEntry;
        }

        public void Delete(long id)
        {
            Dictionary.Remove(id);
        }

        public TimeEntry Find(long id)
        {
            return Dictionary.GetValueOrDefault(id);
        }

        public IEnumerable<TimeEntry> List()
        {
            var list = new List<TimeEntry>();
            foreach(var pair in Dictionary) {
                list.Add(pair.Value);
            }
            return list;
        }

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            timeEntry.Id = id;
            Dictionary[id] = timeEntry;
            return timeEntry;
        }
    }
}