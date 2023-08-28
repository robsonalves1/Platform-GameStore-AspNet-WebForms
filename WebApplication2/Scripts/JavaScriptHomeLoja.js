function BtnExibirProduto(id) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/ShowSelectDepartment",
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

function BtnLoginCliente() {
    $.ajax({
        type: "POST",
        url: "HomeLoja.aspx/BtnLoginCliente",
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

function BtnLogoutCliente() {
    $.ajax({
        type: "POST",
        url: "HomeLoja.aspx/BtnLogoutCliente",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location.reload();
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function ShowListOfSearchedProduct(filtro) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/ShowListOfSearchedProduct",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{filtro: "' + filtro + '" }',
        success: function (response) {
            if (response.d == "") {
                document.getElementById("InputSearchBar").value = "";
                document.getElementById("DivResultados").style.visibility = 'hidden';
                document.getElementById("DivIconApagaResultados").style.visibility = 'hidden';
            } else {
                document.getElementById("DivResultados").style.visibility = 'visible';
                document.getElementById("DivIconApagaResultados").style.visibility = 'visible';
                document.getElementById("DivResultados").innerHTML = response.d;
            }
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function LimpaShowListOfSearchedProduct() {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/LimpaShowListOfSearchedProduct",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            document.getElementById("DivResultados").style.visibility = 'hidden';
            document.getElementById("DivResultados").innerHTML = "";
            console.log(response.d);
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function BtnGoToGamesPage() {
    $.ajax({
        type: "POST",
        url: "HomeLoja.aspx/BtnGoToGamesPage",
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


function BtnGoToPublishersPage() {
    $.ajax({
        type: "POST",
        url: "HomeLoja.aspx/BtnGoToPublishersPage",
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