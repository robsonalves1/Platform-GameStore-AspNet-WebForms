<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="HomePublishers.aspx.cs" Inherits="WebApplication2.View.HomePublishers" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLojaJogos.css" rel="stylesheet" />
    <link href="../Content/StyleSheetHomePublishers.css" rel="stylesheet" />
    <title>Home Publishers</title>
</head>
<body>
    <form id="form1" runat="server">
        <%--NAVBAR--%>
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" style="color: white;" href="LojaJogos.aspx">Loja de Jogos</a>
                <div id="DivSearchBar" class="d-flex" runat="server">
                    <input class="form-control mx-2" type="search" placeholder="Search" aria-label="search" id="InputSearchBar" autocomplete="off" runat="server" />
                    <div id="DivIconApagaResultados" runat="server">
                        <button type="button" id="BtnIconApagaResultado" runat="server" onserverclick="BtnIconEraseSearchedResults_Click">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                                <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z" />
                            </svg>
                        </button>
                    </div>
                    <div id="DivIconSearch">
                        <button type="button" runat="server" id="BtnSearchGames" onserverclick="BtnSearchGames_Click">
                            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-search text-white" viewBox="0 0 16 16">
                                <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                            </svg>
                        </button>
                    </div>
                </div>

                <ul>
                    <li class="button-dropdown">
                        <a href="javascript:void(0)" class="dropdown-toggle" id="DropdownConta" runat="server">Faça login<span><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-person-circle" viewBox="0 0 16 16">
                            <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
                            <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z" />
                        </svg></span>
                        </a>
                        <ul id="DropdownContaMenu" class="dropdown-menu" runat="server">
                        </ul>
                    </li>
                </ul>
            </div>
        </nav>

        <div id="DivResultados" runat="server">
        </div>

        <div id="DivAlert" runat="server">
            <label id="LblAlert" runat="server"></label>
        </div>

        <div class="container text-center">
            <h1>Lista de desenvolvedoras</h1>
            <div id="DivResultPublishers" class="row list-group" runat="server">                
            </div>
        </div>
        <div class="container text-center mt-5">
            <button type="button" class="btn btn-secondary" id="BtnLastPageHomePublishers" runat="server" onserverclick="BtnLastPageHomePublishers_Click">Anterior</button>
            <button type="button" class="btn btn-primary" id="BtnNextPageHomePublishers" runat="server" onserverclick="BtnNextPageHomePublishers_Click">Próximo</button>
        </div>

        <!-- FOOTER -->
        <footer class="container text-center">
            <p class="float-end">
                <a href="#">
                    <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-chevron-double-up" viewBox="0 0 16 16">
                        <path fill-rule="evenodd" d="M7.646 2.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 3.707 2.354 9.354a.5.5 0 1 1-.708-.708l6-6z" />
                        <path fill-rule="evenodd" d="M7.646 6.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 7.707l-5.646 5.647a.5.5 0 0 1-.708-.708l6-6z" />
                    </svg>
                </a>
            </p>
            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Jogos</p>
        </footer>
    </form>

    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.6.4.min.js"></script>
    <script src="../Scripts/JavaScriptHomePublishers.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>
</body>
</html>
