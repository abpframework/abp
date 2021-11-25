using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.App;

public class LocalizationTestController : AbpController
{
    public IActionResult HelloJohn()
    {
        // ReSharper disable once Mvc.ViewNotResolved
        return View();
    }

    public IActionResult PersonForm()
    {
        // ReSharper disable once Mvc.ViewNotResolved
        return View(new PersonModel());
    }

    public class PersonModel
    {
        public string BirthDate { get; set; }

        public string BirthDate1 { get; set; }

        public string BirthDate2 { get; set; }

        public string BirthDate3 { get; set; }

        public PersonModel()
        {
            BirthDate = DateTime.Now.ToString("yyyy-MM-dd");
            BirthDate1 = BirthDate;
            BirthDate2 = BirthDate;
            BirthDate3 = BirthDate;
        }
    }
}
