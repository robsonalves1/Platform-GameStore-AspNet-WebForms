<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveAccessPage.aspx.cs" Inherits="WebApplication2.View.ApproveAccessPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve Access</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetHome.css" rel="stylesheet" />
</head>
<body>
    <header class="site-header sticky-top py-1" id="HeaderHome">
        <nav class="container d-flex flex-column flex-md-row justify-content-between">
            <a class="py-2" href="ApproveAccessPage.aspx" aria-label="Product" id="LogoLoja">Central de Controle
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
            <div id="DivAlert" class="alert alert-danger position-absolute top-50 start-0" runat="server" visible="false">
                <label id="LblAlert" runat="server">
                </label>
            </div>
            <div id="DivAccountsUser" runat="server">
                <h1>Contas usuários</h1>
                <table class="table" id="TableAccountsUser">
                    <tbody id="TBodyAccountsUser" runat="server">
                    </tbody>
                </table>
            </div>
            <div id="DivAccountsClient" runat="server">
                <h1>Contas Clientes</h1>
                <table class="table" id="TableAccountsClient">
                    <tbody id="TBodyAccountsClient" runat="server">
                    </tbody>
                </table>
            </div>
        </div>

        <%--MODAL USER ACCOUNT DETAILS--%>
        <div class="modal fade" id="ModalContaUserDetails" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="ModalTitleUserDetails">Detalhes da Conta</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" runat="server"></button>
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
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal" runat="server">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL APPROVE ACCOUNT -->
        <div class="modal fade" id="ModalApproveAccount" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="ModalApproveAccountTitle">Aprovar Cadastro</h1>
                        <label runat="server" id="LblInvisibleApproveAccount" visible="false"></label>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        Deseja aprovar cadastro?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">não</button>
                        <button type="button" class="btn btn-primary" id="BtnConfirmAccount" runat="server" onserverclick="BtnConfirmAccount_Click">Sim</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- MODAL REJECT ACCOUNT -->
        <div class="modal fade" id="ModalRejectAccount" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="ModalRejectAccountTitle">Rejeitar Cadastro</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        Deseja rejeitar cadastro?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">não</button>
                        <button type="button" class="btn btn-primary" runat="server" id="BtnRejectAccount" onserverclick="BtnRejectAccount_Click">Sim</button>
                    </div>
                </div>
            </div>
        </div>



        <%--INVISIBLE LABEL TO SELECT Nº OF THE LINE TO OPEN THE CORRECT MODAL--%>
        <asp:HiddenField ID="ValueHiddenField"
            Value=""
            runat="server" />

        <footer class="my-5 pt-5 text-body-secondary text-center text-small">
            <p class="mb-1">© 2023 Loja de Informática</p>
        </footer>
    </form>

    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.6.4.min.js"></script>
    <script src="../Scripts/JavaScriptApproveAccessPage.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>
</body>
</html>
