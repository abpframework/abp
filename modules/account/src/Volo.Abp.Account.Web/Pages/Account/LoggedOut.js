document.addEventListener("DOMContentLoaded", function (event) {
    setTimeout(function () {
        var redirectButton = document.getElementById("redirectButton");
        
        if(!redirectButton){
            return;
        }
            
        redirectButton.getAttribute("cname");
        redirectButton.getAttribute("href");
    }, 3000)
});
