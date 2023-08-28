<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="WebApplication2.View.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Produtos</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetHome.css" rel="stylesheet" />
</head>
<body>
    <header class="site-header sticky-top py-1" id="HeaderHome">
        <nav class="container d-flex flex-column flex-md-row justify-content-between">
            <a class="py-2" href="Home.aspx" aria-label="Product" id="LogoLoja">Estoque Loja
            </a>

            <ul>
                <li class="button-dropdown">
                    <a href="javascript:void(0)" class="dropdown-toggle" id="DropdownConta" runat="server"><span>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-circle" viewBox="0 0 16 16">
                            <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
                            <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z" />
                        </svg></span>
                    </a>
                    <ul id="DropdownContaMenu" class="dropdown-menu" runat="server">
                    </ul>
                </li>
            </ul>

        </nav>
    </header>

    <form id="form1" runat="server">
        <div class="container">
            <div class="col-sm-12 mt-3">
                <h1>Lista de Produtos</h1>
                <%--BUTTON MODAL ADD PRODUCT--%>
                <button type="button" class="w-100 btn btn-primary btn-lg mb-3" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                    Adicionar Produto
                </button>

                <button type="button" class="btn btn-primary pb-1" id="BtnMostraFiltros" onclick="DropdownFiltroProduto()">
                    Filtros <span>
                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-right-fill" viewBox="0 0 16 16">
                            <path d="m12.14 8.753-5.482 4.796c-.646.566-1.658.106-1.658-.753V3.204a1 1 0 0 1 1.659-.753l5.48 4.796a1 1 0 0 1 0 1.506z" />
                        </svg></span>
                </button>
                <div id="DivFiltersProducts" class="mt-1" runat="server"></div>
            </div>

            <%--DIV PRODUCT--%>
            <div id="DivProdutos" runat="server">
                <table class="table" id="TableProdutos">
                    <tbody id="TBodyProdutos" runat="server">
                    </tbody>
                </table>
            </div>

            <%--DIVALERT ERRORS--%>
            <div id="divAlert" runat="server" class="alert alert-danger position-absolute top-50 start-0" visible="false">
                <label id="lblAlert" runat="server"></label>
            </div>

        </div>

        <%--MODAL USER ACCOUNT DETAILS--%>
        <div class="modal fade" id="modalContaDetalhes" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="ContaDetalhesTitle">Detalhes da Conta</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server" onserverclick="BtnCancelAddProduct_Click"></button>
                    </div>
                    <div class="modal-body">
                        <div id="DivContaDetalhes">
                            <div id="div1" runat="server" class="alert alert-danger mb-3" visible="false">
                                <label id="Label2" runat="server"></label>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Nome</label>
                                <input id="TxbContaNome" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Senha</label>
                                <input id="TxbContaSenha" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Cargo</label>
                                <input id="TxbContaCargo" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Departamento</label>
                                <select class="form-select" aria-label="Default select example" id="SelectDepartamento" runat="server">
                                    <option>Selecione o departamento</option>
                                    <option value="Diretoria">Diretoria</option>
                                    <option value="Tecnologia">Tecnologia</option>
                                    <option value="Financas">Finanças</option>
                                    <option value="Comercial">Comercial</option>
                                    <option value="Marketing">Marketing</option>
                                    <option value="RH">RH</option>
                                </select>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Data de Admissão</label>
                                <input id="TxbContaAdmissao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary" type="button" runat="server" id="BtnChangeAccountDetails" onserverclick="BtnChangeAccountDetails_Click">Salvar</button>
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="Button5" onserverclick="BtnCancelAddProduct_Click">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL NEW PRODUCT-->
        <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="staticBackdropLabel">Novo Produto</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server" onserverclick="BtnCancelAddProduct_Click"></button>
                    </div>
                    <div class="modal-body">
                        <div id="divAddProduto">
                            <div id="div2" runat="server" class="alert alert-danger mb-3" visible="false">
                                <label id="AlertForm" runat="server"></label>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Código</label>
                                <input id="TxbCodigo" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Quantidade</label>
                                <input id="TxbQuantidade" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Marca</label>
                                <input id="TxbMarca" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Nome</label>
                                <input id="TxbNome" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Valor</label>
                                <input type="text" class="form-control" id="TxbPreco" runat="server" value='<%# string.Format("{0:C}", Eval("CreditoDisponivel")) %>' />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Departamento</label>
                                <input type="text" class="form-control" id="TxbDepartamento" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Características Técnicas</label>
                                <textarea class="form-control" id="TxbCaracteristica" runat="server"></textarea>
                            </div>
                            <div class="mb-3">
                                <label for="formFile" class="form-label">Imagem</label>
                                <asp:FileUpload ID="FotoUpload" class="form-control" runat="server" multiple="multiple" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary" type="submit" runat="server" id="btnAddProduto" onserverclick="BtnAddProduct_Click">Salvar</button>
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="btnCancelarAddProduto" onserverclick="BtnCancelAddProduct_Click">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL DELETE PRODUCT-->
        <div class="modal fade" id="modalExluirProduto" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="staticBackdropeExcluirLabel">Excluir do Produto</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server" onserverclick="BtnCancelAddProduct_Click"></button>
                    </div>
                    <div class="modal-body">
                        <div id="divExcluirProduto">
                            Deseja excluir produto?
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" runat="server" id="btnExcluirProduto" onserverclick="BtnDeleteProduct_Click">Sim</button>
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="Button2">Não</button>
                    </div>
                </div>
            </div>
        </div>

        <%--MODAL DESCRIPTION--%>
        <div class="modal fade" id="modalDescricao" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="staticBackdropDescricaoLabel">Descrição do Produto</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server" onserverclick="BtnCancelAddProduct_Click"></button>
                    </div>
                    <div class="modal-body">
                        <div id="divDescricaoProduto">
                            <div id="div3" runat="server" class="alert alert-danger mb-3" visible="false">
                                <label id="Label1" runat="server"></label>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Código</label>
                                <input id="TxbCodigoDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Quantidade</label>
                                <input id="TxbQuantidadeDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Marca</label>
                                <input id="TxbMarcaDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Nome</label>
                                <input id="TxbNomeDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Valor</label>
                                <input id="TxbValorDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Departamento</label>
                                <input id="TxbDepartamentoDescricao" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="Button3">Voltar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL UPDATE PRODUCT-->
        <div class="modal fade" id="modalEditarProduto" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="staticBackdropEditarLabel">Editar do Produto</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server" onserverclick="BtnCancelAddProduct_Click"></button>
                    </div>
                    <div class="modal-body">
                        <div id="divEditarProduto">
                            <div id="divAlertaEditar" runat="server" class="alert alert-danger mb-3" visible="false">
                                <label id="lblAlertaEditar" runat="server"></label>
                            </div>
                            <div class="mb-3" visible="false">
                                <label class="form-label">Id</label>
                                <input id="TxbIdEditar" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Código</label>
                                <input id="TxbCodigoEditar" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Quantidade</label>
                                <input id="TxbQuantidadeEditar" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Marca</label>
                                <input id="TxbMarcaEditar" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Nome</label>
                                <input id="TxbNomeEditar" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Valor</label>
                                <input type="text" class="form-control" id="TxbValorEditar" runat="server" value='<%# string.Format("{0:C}", Eval("CreditoDisponivel")) %>' />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Departamento</label>
                                <input type="text" class="form-control" id="TxbDepartamentoEditar" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Características Técnicas</label>
                                <textarea class="form-control" id="TxbCaracteristicasEditar" runat="server"></textarea>
                            </div>
                            <div class="mb-3">
                                <label for="formFile" class="form-label">Imagem</label>
                                <asp:FileUpload ID="FotoUploadEditar" class="form-control" runat="server" multiple="multiple" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" runat="server" id="btnEditarProduto" onserverclick="BtnUpdateProduct_Click">Editar</button>
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="Button1">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <%--INVISIBLE LABEL TO SELECT Nº OF THE LINE TO OPEN THE CORRECT MODAL--%>
        <asp:HiddenField ID="ValueHiddenField"
            Value=""
            runat="server" />

        <%--INVISIBLE LABEL TO INSERT ID FROM THE COLUMN ID--%>
        <asp:HiddenField ID="HiddenFieldIdEditar"
            Value=""
            runat="server" />

        <footer class="my-5 pt-5 text-body-secondary text-center text-small">
            <p class="mb-1">© 2023 Loja de Informática</p>
        </footer>
    </form>

    <script src="../Scripts/jquery-3.6.4.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/JavaScriptHome.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>

    <%--AJAX SCRIPT--%>
    <script>
        function BtnLogoutUser() {
            $.ajax({
                type: "POST",
                url: "Home.aspx/BtnLogoutUser",
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

        function ApplySelectedFilter() {
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
                url: "Home.aspx/ApplySelectedFilter",
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
    </script>

</body>
</html>
