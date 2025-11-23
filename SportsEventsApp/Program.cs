using SportsEventsApp.Models;
using SportsEventsApp.Repositories;

// minimal console app example
string connString = "Server= DESKTOP-CLNPDP4\\SQLEXPRESS; Database=SportsEvents; Trusted_Connection=True;";
using var uow = new UnitOfWork(connString);

try
{
    // 1) Upsert athlete
    var athlete = new Athlete
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Country = "Ukraine",
        Dob = new DateTime(1995, 5, 1)
    };

    int userId = 1; // id користувача який виконує операцію (created_by)
    int athleteId = await uow.Athletes.UpsertAsync(athlete, userId);
    Console.WriteLine($"Athlete upserted id = {athleteId}");

    // 2) Sell tickets (in same transaction)
    int buyerId = 2;
    int ticketId = 1; // переконайся що такий квиток існує
    int qty = 2;
    int orderId = await uow.Tickets.SellTicketsAsync(buyerId, ticketId, qty, userId);
    Console.WriteLine($"Order created id = {orderId}");

    // commit transaction
    await uow.CommitAsync();
    Console.WriteLine("Transaction committed.");
}
catch (Exception ex)
{
    uow.Rollback();
    Console.WriteLine("Error: " + ex.Message);
}
finally
{
    uow.Dispose();
}
