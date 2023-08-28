function BtnExibirProduto(id) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/GoToSelectedProductPage",
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

function FilterProducts(filtro) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/FilterProducts",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{filtro: "' + filtro + '" }',
        success: function (response) {

            document.getElementById("DivTodosProdutos").innerHTML = response.d;
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

function ResultSearchedProduct(filtro) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/ResultSearchedProduct",
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

function BtnLoginClient() {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/BtnLoginClient",
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

function BtnLogoutClient() {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/BtnLogoutClient",
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