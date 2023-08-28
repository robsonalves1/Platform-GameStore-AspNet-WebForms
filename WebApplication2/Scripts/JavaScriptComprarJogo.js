function BtnShowSelectedGame(id) {
    $.ajax({
        type: "POST",
        url: "ComprarJogo.aspx/BtnShowSelectedGame",
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
        url: "ComprarJogo.aspx/BtnLoginClient",
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
        url: "ComprarJogo.aspx/BtnLogoutClient",
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
            url: "ComprarJogo.aspx/SendComment",
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

let imgs = document.getElementById("imgs");

function updateSelected(e, img) {
    let swatches = e.parentElement.children;
    let selected = e.id;

    console.log(img);

    imgs.src = img;

    currentlySelectedColor = colorNameText;
    updateColorText();
    for (var i = 0; i < swatches.length; i++) {
        if (swatches[i].id == selected) {
            swatches[i].className = "swatch selected";
        } else {
            swatches[i].className = "swatch";
        }
    }
    showButton();
}

function showButton() {
    button.style.opacity = "1";
    button.style.height = "45px";
}