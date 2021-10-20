(
    function () {
        var isRtl = JSON.parse(localStorage.getItem("Abp.IsRtl"));
        var htmlTag = document.getElementsByTagName("html")[0];

        if (htmlTag) {
            var selectedLanguage = localStorage.getItem("Abp.SelectedLanguage");
            if (selectedLanguage) {
                htmlTag.setAttribute("lang", selectedLanguage);
            }

            if (isRtl) {
                htmlTag.setAttribute("dir", "rtl");
            }
        }
    }
)();