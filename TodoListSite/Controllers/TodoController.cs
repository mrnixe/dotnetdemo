using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListSite.Services;
using TodoListSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace TodoListSite.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _TodoItemService;
        private readonly UserManager<IdentityUser> _UserManager;

        public TodoController(ITodoItemService TodoItemService,
            UserManager<IdentityUser> UserManager)
        {
            _TodoItemService = TodoItemService;
            _UserManager = UserManager;
        }

        // 一个 action 方法可以返回视图、JSON数据，
        // 或者 200 OK和404 Not Found 之类的状态码。返回类型 IActionResult 
        // 给了你足够的灵活性，以返回上面提到的任意一个。
        public async Task<IActionResult> Index()
        {
            var currentUser = await _UserManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            // 从数据库获取 to-do 条目
            var items = await _TodoItemService.GetIncompleteItemsAsync(currentUser);


            // 把条目置于 model 中
            var model = new TodoViewModel()
            {
                Items = items
            };

            // 使用 model 渲染视图
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _UserManager.GetUserAsync(User);
            if (currentUser==null ) return Challenge();

            var successful = await _TodoItemService.AddItemAsync(newItem, currentUser);
            if (!successful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id )
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _UserManager.GetUserAsync(User);
            if (currentUser==null ) return Challenge();

            var successful = await _TodoItemService.MarkDoneAsync(id, currentUser );
            if (!successful)
            {
                return BadRequest("Could not mark item as done.");
            }

            return RedirectToAction("Index");
        }


    }
}