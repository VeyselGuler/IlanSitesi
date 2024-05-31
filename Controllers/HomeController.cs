using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IlanSitesi.Models;

namespace IlanSitesi.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(IlaniGetir());
    }
    
    [HttpGet]
    public IActionResult Editor(string filter = "")
    {
        ViewData["filter"] = filter;
        return View(IlaniGetir());
    }

    [HttpPost]
    public IActionResult Editor(IlanModel model)
    {
        var ilanlar = IlaniGetir();

        using StreamReader reader = new("App_Data/index.txt");
        var txt = reader.ReadToEnd();
        reader.Close();

        if (!string.IsNullOrEmpty(txt))
        {
            txt += "\n";
        }

        using StreamWriter writer = new("App_Data/index.txt");
        writer.Write($"{txt}{model.Name}|{model.Price}|{model.Img}|{DateTime.Now}");
        ilanlar.Add(model);

        return View(ilanlar);
    }


    public List<IlanModel> IlaniGetir()
    {
        var ilanlar = new List<IlanModel>();
        using StreamReader reader = new("App_Data/index.txt");
        var txt = reader.ReadToEnd();

        var Lines = txt.Split('\n');
        foreach (var ilanLine in Lines)
        {
            var parts = ilanLine.Split('|');
            ilanlar.Add(
                new IlanModel()
                {
                    Name = parts[0],
                    Price = int.Parse(parts[1]),
                    Img = parts[2],
                    Tarih = DateTime.Parse(parts[3])
                }
            );
        }

        return ilanlar;
    }
}