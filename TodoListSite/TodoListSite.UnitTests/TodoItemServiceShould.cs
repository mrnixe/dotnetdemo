using System;
using System.Threading.Tasks;
using TodoListSite.Data;
using TodoListSite.Models;
using TodoListSite.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.InMemory;
using Xunit;

namespace TodoListSite.UnitTests
{
    public class TodoItemServiceShould
    {
        [Fact]
        public async Task AddNewItemAsIncompleteWithDueDate()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_AddNewItem").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new TodoItemService(context);

                var fakeUser = new IdentityUser

                {
                    Id = "fake-000",
                    UserName = "fake@example.com"
                };

                await service.AddItemAsync(new TodoItem
                {
                    Title = "Testing?"
                }, fakeUser);


                using (var InMemorycontext = new ApplicationDbContext(options))
                {
                    var itemsInDatabase = await InMemorycontext
                        .Items.CountAsync();
                    Assert.Equal(1, itemsInDatabase);

                    var item = await InMemorycontext.Items.FirstAsync();
                    Assert.Equal("Testing?", item.Title);
                    Assert.Equal(false, item.IsDone);

                    // Item should be due 3 days from now (give or take a second)
                    var difference = DateTimeOffset.Now.AddDays(3) - item.DueAt;
                    Assert.True(difference < TimeSpan.FromSeconds(1));

                }


            }
        }



    }
}