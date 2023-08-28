<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomeContaCliente.aspx.cs" Inherits="WebApplication2.View.HomeContaCliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <title>Conta Cliente</title>
</head>
<body>
    <form id="form1" runat="server">
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" href="HomeLoja.aspx" style="color: white;">Loja</a>
            </div>
        </nav>
        <div class="container border border-2 mt-5" id="DivContaCliente" runat="server">
            <div class="text-center">
                <div id="DivAlert" runat="server" visible="false">
                    <label id="LblAlert" runat="server"></label>
                </div>
                <div class="mb-3">
                    <h1>Editar conta</h1>
                </div>
                <div class="mb-3">
                    <span>Nome: </span>
                    <p id="TxbNome" runat="server">Test</p>
                </div>
                <div class="mb-3">
                    <span>Email: </span>
                    <p id="TxbEmail" runat="server">test@email.com</p>
                </div>

                <h3 class="mt-4">Produtos Comprados</h3>

                <table class="table my-5">
                    <thead id="THeadProdutosComprados" runat="server">
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">Foto</th>
                            <th scope="col">Produto</th>
                            <th scope="col">Quantidade</th>
                        </tr>
                    </thead>
                    <tbody id="TbodyProdutosComprados" runat="server">
                        
                    </tbody>
                </table>

                <div class="mb-3">
                    <button type="button" class="btn btn-secondary col-6 mb-2" runat="server" id="BtnVoltaPaginaAnterior" onserverclick="BtnPreviousPage_Click">Voltar</button>
                    <button type="button" class="btn btn-primary col-6" runat="server" id="BtnPaginaModificar" data-bs-toggle="modal" data-bs-target="#modalModificaUsuario">Modificar</button>
                </div>
            </div>
        </div>

        <div id="DivAlertContaAtualizada" runat="server" class="alert alert-success mb-3 " visible="false">
            Conta atualizada com sucesso!
        </div>

        <%--MODAL MODIFICA CLIENTE--%>
        <div class="modal fade" id="modalModificaUsuario" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="staticBackdropLabel">Conta</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server"></button>
                    </div>
                    <div class="modal-body">
                        <div id="DivModificaProduto">
                            <div id="div2" runat="server" class="alert alert-danger mb-3" visible="false">
                                <label id="AlertForm" runat="server"></label>
                            </div>
                            <div class="mb-3">
                                <label for="exampleInputEmail1" class="form-label">Nome</label>
                                <input id="TxbNomeModificar" type="text" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label for="exampleInputEmail1" class="form-label">Email</label>
                                <input id="TxbEmailModificar" type="text" class="form-control" runat="server" disabled="disabled" />
                            </div>
                            <div class="mb-3">
                                <label for="exampleInputEmail1" class="form-label">Senha Antiga</label>
                                <input id="TxbSenhaAntigaModificar" type="password" class="form-control" runat="server" />
                            </div>
                            <div class="mb-3">
                                <label for="exampleInputEmail1" class="form-label">Senha Atual</label>
                                <input id="TxbSenhaAtualModificar" type="password" class="form-control" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button class="btn btn-primary" type="submit" runat="server" id="BtnModificarCliente" onserverclick="BtnModifyAccountClient_Click">Salvar</button>
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server" id="btnCancelarModificarCliente">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="../Scripts/bootstrap.min.js"></script>
</body>
</html>
