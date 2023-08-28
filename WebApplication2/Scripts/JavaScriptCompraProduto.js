let qtd = document.getElementById("CampoProdutoQuantidade").innerHTML;

function CloseDivAlert() {
    let DivAlertAjax = document.getElementById("DivAlertAjax");
    let DivAlert = document.getElementById("DivAlert");

    DivAlertAjax.style.visibility = "hidden";
    DivAlert.style.visibility = "hidden";
}

function BtnExibirProduto(id) {
    $.ajax({
        type: "POST",
        url: "Loja.aspx/MostraProdutoSelecionado",
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

function ComprarProduto() {
    $.ajax({
        type: "POST",
        url: "ComprarProduto.aspx/BtnBuyProduct",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccess,
        failure: function (response) {
            alert(response.d);
        }
    });
}

function OnSuccess(response) {
    let res = response.d;
    if (res == 0)
        alert("Produto não possui mais estoque disponível.");
    else if (res == "Cliente deve estar logado para comprar!") {
        document.getElementById("DivAlertAjax").style.visibility = "visible";
        document.getElementById("LabelAlertAjax").innerHTML = "Cliente deve estar logado para comprar!";
    }
    else
        document.getElementById("CampoProdutoQuantidade").innerHTML = res;

}

function ShowListSearchProduct(filtro) {
    $.ajax({
        type: "POST",
        url: "ComprarProduto.aspx/ShowListSearchProduct",
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
        url: "ComprarProduto.aspx/BtnLoginClient",
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
        url: "ComprarProduto.aspx/BtnLogoutClient",
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

function SendComment() {
    if (document.getElementById("TxbComentarioProduto").value != "" && document.getElementById("TxbNotaProduto").value != "") {
        $.ajax({
            type: "POST",
            url: "ComprarProduto.aspx/SendComment",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{comentario: "' + document.getElementById("TxbComentarioProduto").value + '", nota: "' + document.getElementById("TxbNotaProduto").value + '"}',
            success: function (response) {
                document.getElementById("SectionComentarios").innerHTML = response.d[0];
                document.getElementById("CampoProdutoNotaEstrelas").innerHTML = response.d[1];
                document.getElementById("CampoProdutoNotaQuantidadeDeComentarios").innerHTML = "(" + response.d[2] + ")";

                // IF RETURN ERRORS
                let DivAlertAjax = document.getElementById("DivAlertAjax");
                let LabelAlertAjax = document.getElementById("LabelAlertAjax");
                LabelAlertAjax.innerHTML = response.d[3];
                if (response.d[3] != null)
                    DivAlertAjax.style.visibility = "visible"
            },
            failure: function (response) {
                console.log(response.d[2]);
            }
        });
    } else {
        alert("Preencha todos os campos corretamente.");
    }
}