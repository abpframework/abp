namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.JsTree
{
    public class JsTreeOptions
    {
        /// <summary>
        /// Path of the style file for the JsTree library.
        /// Setting to null ignores the style file.
        /// 
        /// Default value: "/libs/jstree/themes/default/style.min.css".
        /// </summary>
        public string StylePath { get; set; } = "/libs/jstree/themes/default/style.min.css";
    }
}