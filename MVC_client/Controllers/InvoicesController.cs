using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Client.Models;

namespace MVC_Client.Controllers;

public class InvoicesController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private readonly IConfiguration _configuration;
	private readonly string _invoicesAPIBaseURL;

	public InvoicesController(ILogger<HomeController> logger, IConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;

		// _invoicesAPIBaseURL = _configuration.GetValue<string>("InvoicesAPIBaseURL");
		_invoicesAPIBaseURL = _configuration.GetValue<string>("InvoicesAPIBaseURL");
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		using (HttpClient client = new HttpClient())
		{
			using (HttpResponseMessage response = await client.GetAsync(_invoicesAPIBaseURL))
			{
				if (response.StatusCode.Equals(HttpStatusCode.OK))
				{
					IEnumerable<InvoiceResponseModel> responseViewModel = await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceResponseModel>>();
					return View(responseViewModel);
				}
				else
				{
					ModelState.Clear();
					ModelState.AddModelError(string.Empty, response.StatusCode.ToString());
					return View();
				}
			}
		}
	}

	[HttpGet]
	public async Task<ActionResult> Create()
	{
		ViewData["PaymentMethods"] = await LoadPaymentMethods();
		ViewData["Categories"] = await LoadCategories();

		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<ActionResult> Create(CreateInvoiceRequestModel viewModel)
	{
		if (!ModelState.IsValid)
		{
			ModelState.AddModelError("", "Modelo inválido.");
		}

		using (HttpClient client = new HttpClient())
		{
			string endpoint = _invoicesAPIBaseURL;
			StringContent content = new StringContent(JsonSerializer.Serialize(viewModel), Encoding.UTF8, "application/json");

			using (HttpResponseMessage response = await client.PostAsync(endpoint, content))
			{
				if (response.StatusCode.Equals(HttpStatusCode.Created))
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ModelState.AddModelError("", await response.Content.ReadAsStringAsync());

					ViewData["paymentMethods"] = await LoadPaymentMethods();
					ViewData["categories"] = await LoadCategories();

					return View(viewModel);
				}
			}
		}
	}

	[HttpGet]
	public ActionResult Edit(int id)
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Edit(int id, IFormCollection collection)
	{
		try
		{
			return RedirectToAction(nameof(Index));
		}
		catch
		{
			return View();
		}
	}

	[HttpGet]
	public ActionResult Delete(int id)
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Delete(int id, IFormCollection collection)
	{
		try
		{
			return RedirectToAction(nameof(Index));
		}
		catch
		{
			return View();
		}
	}

	private async Task<List<SelectListItem>> LoadPaymentMethods()
	{
		using (HttpClient client = new HttpClient())
		{
			using (HttpResponseMessage response = await client.GetAsync(_invoicesAPIBaseURL + "/paymentmethods"))
			{
				if (response.StatusCode.Equals(HttpStatusCode.OK))
				{
					Dictionary<string, string> responsePaymentMethods = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
					List<SelectListItem> paymentMethods = responsePaymentMethods.Select(pm => new SelectListItem { Text = pm.Value, Value = pm.Key }).ToList();
					return paymentMethods;
				}
				else
				{
					return new List<SelectListItem>();
				}
			}
		}
	}

	private async Task<List<SelectListItem>> LoadCategories()
	{
		using (HttpClient client = new HttpClient())
		{
			using (HttpResponseMessage response = await client.GetAsync(_invoicesAPIBaseURL + "/categories"))
			{
				if (response.StatusCode.Equals(HttpStatusCode.OK))
				{
					IEnumerable<InvoiceCategory> responseCategories = await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceCategory>>();
					List<SelectListItem> categories = responseCategories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
					return categories;
				}
				else
				{
					return new List<SelectListItem>();
				}
			}
		}
	}
}
