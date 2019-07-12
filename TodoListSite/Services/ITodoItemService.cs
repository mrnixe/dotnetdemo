using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListSite.Models;

namespace TodoListSite.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync();
        Task<bool> AddItemAsync(TodoItem newItem);
    }
}