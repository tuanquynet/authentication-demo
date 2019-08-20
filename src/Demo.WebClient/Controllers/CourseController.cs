using Demo.WebClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Demo.WebClient.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly string url = "https://localhost:2500/api/courses";
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetAll(string sidx, string sord, int page, int rows, bool _search, string searchField, string searchOper, string searchString)
        {
            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var listResults = await GetAllAsync();
            int totalRecords = listResults.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = listResults
            };

            return Json(jsonData);
        }

        private async Task<IEnumerable<CourseModel>> GetAllAsync()
        {
            List<CourseModel> course = new List<CourseModel>();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            Console.WriteLine("Access Token: " + accessToken);
            Console.WriteLine("Refresh Token: " + refreshToken);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    course = JsonConvert.DeserializeObject<List<CourseModel>>(Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync()));
                }
            }
            //return Json(jsonData, JsonRequestBehavior.AllowGet);
            return course;
        }
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Add(CourseModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/course");
                }
                else
                {
                    ModelState.AddModelError(response.StatusCode.ToString(), response.ToString());
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var courses = await GetAllAsync();
            return View(courses.FirstOrDefault(x => x.Id == id));
        }
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Edit(CourseModel model)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.PutAsync(url+$"/{model.Id}", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/course");
                }
                ModelState.AddModelError(response.StatusCode.ToString(), response.ToString());
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.DeleteAsync(url + $"/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/course");
                }
                ModelState.AddModelError(response.StatusCode.ToString(), response.ToString());
            }
            var courses = await GetAllAsync();
            return View(courses.FirstOrDefault(x => x.Id == id));
        }
    }
}