using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EventEase.Models;

namespace EventEase.Services;
public class EventService
{
    private readonly List<EventDetails> events = new()
    {
        new EventDetails { Id = 1, Name = "Tech Conference", Date = new DateTime(2023, 11, 15), Location = "New York" },
        new EventDetails { Id = 2, Name = "Music Festival", Date = new DateTime(2023, 12, 5), Location = "Los Angeles" }
    };

    public Task<EventDetails> GetEventByIdAsync(int eventId)
    {
        return Task.FromResult(events.FirstOrDefault(e => e.Id == eventId));
    }

    public void AddEvent(EventDetails newEvent)
    {
        newEvent.Id = GetNextEventId();
        events.Add(newEvent);
    }

    public List<EventDetails> GetAllEvents()
    {
        return events;
    }

    public Task<List<EventDetails>> GetAllEventsAsync()
    {
        return Task.FromResult(events);
    }

    private int GetNextEventId()
    {
        return events.Any() ? events.Max(e => e.Id) + 1 : 1;
    }
}