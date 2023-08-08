let btnCarregaDescricao = document.getElementById("btnCarregaDescricao");
let btnMostraDescricao = document.getElementById("btnMostraDetalhes");
let estaVisivel = false;
let filtroEstoqueMarca = [];
let filtroEstoquePreco = "";
let idDB = "";

function getBtnId(event) {
    let btn = event.target;
    let id = btn.getAttribute("data-id");
    ValueHiddenField.value = id;
}

function btnExcluir_click(event) {
    getBtnId(event);
    let btn = event.target;

    idDB = btn.getAttribute("id");
    ValueHiddenField.value = idDB;
}

function btnDescricao_click(event) {
    // PEGA O ID DA LINHA A SER MOSTRADA
    getBtnId(event);
    let id = ValueHiddenField.value;
    console.log(id);

    let tabela = document.querySelector("table#TableProdutos");
    let tr = tabela.rows[id];
    let elementos = tr.querySelectorAll("*");
    document.getElementById("TxbCodigoDescricao").value = elementos[2].innerHTML;
    document.getElementById("TxbQuantidadeDescricao").value = elementos[3].innerHTML;
    document.getElementById("TxbMarcaDescricao").value = elementos[4].innerHTML;
    document.getElementById("TxbNomeDescricao").value = elementos[5].innerHTML;
    document.getElementById("TxbValorDescricao").value = elementos[6].innerHTML;
    document.getElementById("TxbDepartamentoDescricao").value = elementos[7].innerHTML;
}

function btnEditar_click(event) {
    // PEGA O ID DA LINHA A SER MOSTRADA
    getBtnId(event);
    let id = ValueHiddenField.value;
    console.log(id);

    let tabela = document.querySelector("#DivProdutos table");
    let tr = tabela.rows[id];
    let elementos = tr.querySelectorAll("*");
    HiddenFieldIdEditar.value = elementos[1].innerHTML;
    document.getElementById("TxbIdEditar").value = elementos[1].innerHTML;
    document.getElementById("TxbCodigoEditar").value = elementos[2].innerHTML;
    document.getElementById("TxbQuantidadeEditar").value = elementos[3].innerHTML;
    document.getElementById("TxbMarcaEditar").value = elementos[4].innerHTML;
    document.getElementById("TxbNomeEditar").value = elementos[5].innerHTML;
    document.getElementById("TxbValorEditar").value = elementos[6].innerHTML.replace("R$ ", "");
    document.getElementById("TxbDepartamentoEditar").value = elementos[7].innerHTML;

}

function DropdownFiltroProduto() {
    if (!estaVisivel) {
        document.getElementById("BtnMostraFiltros").style.marginBottom = document.getElementById("DivFiltersProducts").offsetHeight + "px";
        document.getElementById("BtnMostraFiltros").innerHTML =
            `Filtros <span>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down-fill" viewBox="0 0 16 16">
                      <path d="M7.247 11.14 2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z"/>
                    </svg>`
        document.getElementById("DivFiltersProducts").style.visibility = "visible";
        estaVisivel = true;
    } else {
        document.getElementById("BtnMostraFiltros").style.marginBottom = "0px";
        document.getElementById("BtnMostraFiltros").innerHTML =
            `Filtros <span>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-right-fill" viewBox="0 0 16 16">
                        <path d="m12.14 8.753-5.482 4.796c-.646.566-1.658.106-1.658-.753V3.204a1 1 0 0 1 1.659-.753l5.48 4.796a1 1 0 0 1 0 1.506z" />
                    </svg></span>`
        document.getElementById("DivFiltersProducts").style.visibility = "hidden";
        estaVisivel = false;
    }
}

function CheckboxMarcaFiltroEstoque(filtro, checkbox) {
    if (checkbox == true) {
        filtroEstoqueMarca.push("'" + filtro + "'");
    } else {
        let idxParaRemover = filtroEstoqueMarca.indexOf(filtro);
        filtroEstoqueMarca.splice(idxParaRemover, 1);
    }

    console.log(filtroEstoqueMarca);
}

function SelectPrecoFiltroEstoque(filtro) {
    filtroEstoquePreco = filtro;
}

function FiltraProdutoEstoque() {
    queryFiltraMarca = "";

    console.log(filtroEstoqueMarca.length);
    console.log(filtroEstoquePreco);

    if (filtroEstoqueMarca.length < 1 && filtroEstoquePreco == "0") {
        console.log("entrou");
        return;
    }

    if (filtroEstoqueMarca.length < 1) {
        queryFiltraMarca = "";
    } else {
        queryFiltraMarca = "produtoMarca IN (" + filtroEstoqueMarca.join(",") + ") AND";
    }

    $.ajax({
        type: "POST",
        url: "Home.aspx/FiltraProdutoEstoque",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{filtroEstoque: "WHERE ' + queryFiltraMarca + ' produtoPreco ' + filtroEstoquePreco + '"}',
        success: function (response) {
            let dt = JSON.parse(response.d);
            let tBodyProdutos = document.getElementById("TBodyProdutos");

            tBodyProdutos.innerHTML =
                "<tr>" +
                "<th scope =\"col\">linha</ th>" +
                "<th scope =\"col\">id</ th>" +
                "<th scope =\"col\">código</ th>" +
                "<th scope =\"col\">quantidade</ th>" +
                "<th scope =\"col\">marca</ th>" +
                "<th scope =\"col\">Nome</ th>" +
                "<th scope =\"col\">preço</ th>" +
                "<th scope =\"col\">Departamento</ th>"
            "<th scope =\"col\">botões</ th>" +
                "</tr>"

            for (let i = 0; i < dt.length; i++) {
                tBodyProdutos.innerHTML += `
                        <tr>
                            <td>${dt[i].linha}</td>
                            <td>${dt[i].produtoId}</td>
                            <td>${dt[i].produtoCodigo}</td>
                            <td>${dt[i].produtoQuantidade}</td>
                            <td>${dt[i].produtoMarca}</td>
                            <td>${dt[i].produtoNome}</td>
                            <td>${dt[i].produtoPreco}</td>
                            <td>${dt[i].produtoDepartamento}</td>
                            <td>
                                <button type="button" id="btnDesejaDeletar" class="btn btn-danger" onclick="getBtnId(event)" data-id="${dt[i].linha}" data-bs-toggle="modal" data-bs-target="#modalExluirProduto">Excluir</button>
                                <button type="button" class="btn btn-info" id="btnMostrarDetalhes" onclick="btnDescricao_click(event)" data-id="${dt[i].linha}" data-bs-toggle="modal" data-bs-target="#modalDescricao">
                                    Detalhes
                                </button>
                                <button type="button" class="btn btn-primary" id="btnEditarProduto" onclick="btnEditar_click(event)" data-id="${dt[i].linha}" data-bs-toggle="modal" data-bs-target="#modalEditarProduto">
                                    Editar
                                </button>
                            </td>
                        </tr>
                        `;
            }

            console.log(response.d);
        },
        failure: function (response) {
            console.log(response.d);
        }
    });
}

