function BtnGoToSelectedGame(id) {
    $.ajax({
        type: "POST",
        url: "HomePublishers.aspx/BtnGoToSelectedGame",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{id: "' + id + '" }',
        success: function (response) {
            window.location.href = response.d;
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function BtnLoginClient() {
    $.ajax({
        type: "POST",
        url: "HomePublishers.aspx/BtnLoginClient",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location.href = response.d;
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}