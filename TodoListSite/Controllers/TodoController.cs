using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListSite.Services;
using TodoListSite.Models;

namespace TodoListSite.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _TodoItemService;
        public TodoController(ITodoItemService TodoItemService)
        {
            _TodoItemService = TodoItemService;
        }

        // 一个 action 方法可以返回视图、JSON数据，
        // 或者 200 OK和404 Not Found 之类的状态码。返回类型 IActionResult 
        // 给了你足够的灵活性，以返回上面提到的任意一个。
        public async Task<IActionResult> Index()
        {
            // 从数据库获取 to-do 条目
            var items = await _TodoItemService.GetIncompleteItemsAsync();


            // 把条目置于 model 中
            var model = new TodoViewModel()
            {
                Items = items
            };

            // 使用 model 渲染视图
            return View(model);
        }
    }
}