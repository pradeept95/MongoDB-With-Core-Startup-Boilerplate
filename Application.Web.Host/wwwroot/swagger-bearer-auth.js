(function () {
    (function () {
        $('#auth_container').append("<input type='text' id='input_apiKey' />");
        $("#input_apiKey").change(addApiKeyAuthorization);
    });

    function addApiKeyAuthorization() {
        var key = encodeURIComponent($('#input_apiKey')[0].value);
        if (key && key.trim() != "") {
            var apiKeyAuth = new SwaggerClient.ApiKeyAuthorization("Authorization", "Bearer " + key, "header");
            window.swaggerUi.api.clientAuthorizations.add("bearer", apiKeyAuth);
        }
    }
})();