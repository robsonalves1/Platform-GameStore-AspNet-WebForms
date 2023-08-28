function getBtnId(event) {
    let btn = event.target;
    let id = btn.getAttribute("data-id");
    ValueHiddenField.value = id;
}

function btnApproveOrRejectAccount_click(event) {
    // GET THE ID OF THE ROW
    getBtnId(event);
    let id = ValueHiddenField.value;
    console.log(id);
}

function BtnLogoutUsuario() {
    $.ajax({
        type: "POST",
        url: "ApproveAccessPage.aspx/BtnLogoutUsuario",
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