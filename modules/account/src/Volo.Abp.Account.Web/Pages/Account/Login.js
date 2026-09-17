$(function () {
    $("#PasswordVisibilityButton").click(function (e) {
        let button = $(this);
        let passwordInput = button.parent().find("input");
        if (!passwordInput) {
            return;
        }

        let isRevealing = passwordInput.attr("type") === "password";
        passwordInput.attr("type", isRevealing ? "text" : "password");
        button.attr("aria-pressed", isRevealing);

        let icon = button.find("i");
        if (icon) {
            icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
        }
    });

});
