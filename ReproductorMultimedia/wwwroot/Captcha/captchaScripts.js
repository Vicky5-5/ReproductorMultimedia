document.addEventListener("DOMContentLoaded", function () {
    if (typeof grecaptcha !== "undefined" && typeof RECAPTCHA_SITEKEY !== "undefined") {
        grecaptcha.ready(function () {
            grecaptcha.execute(RECAPTCHA_SITEKEY, { action: RECAPTCHA_ACTION }).then(function (token) {
                const input = document.createElement("input");
                input.type = "hidden";
                input.name = "g-recaptcha-response";
                input.value = token;

                const form = document.forms["formLogin"];
                if (form) {
                    form.appendChild(input);
                }
            });
        });
    }
});
