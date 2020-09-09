﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Demo.Views.Components.Themes.Shared.Demos.FormElementsDemo
{
    [Widget]
    public class FormElementsDemoViewComponent : AbpViewComponent
    {
        public const string ViewPath = "/Views/Components/Themes/Shared/Demos/FormElementsDemo/Default.cshtml";

        public IViewComponentResult Invoke()
        {
            var model = new FormElementsDemoModel();

            return View(ViewPath, model);
        }
    }
}
