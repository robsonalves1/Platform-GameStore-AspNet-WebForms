﻿<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="LojaJogos.aspx.cs" Inherits="WebApplication2.View.LojaJogos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLojaJogos.css" rel="stylesheet" />
    <title>Loja de Jogos</title>
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
                        <button type="button" id="BtnIconApagaResultado" runat="server" onserverclick="BtnIconApagaResultado_Click">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                                <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z" />
                            </svg>
                        </button>
                    </div>
                    <div id="DivIconSearch">
                        <button type="button" runat="server" id="BtnSearchJogo" onserverclick="BtnSearchJogo_Click">
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
        <div class="container" runat="server">
            <div id="DivJogos" class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-3" runat="server">
            </div>
        </div>
        <div id="DivBtnChangeCurrentPage" class="text-center mt-5" runat="server">
            <button type="button" class="btn btn-secondary" id="BtnPreviusPage" runat="server" onserverclick="BtnPreviusPage_Click">Página Anterior</button>
            <button type="button" class="btn btn-primary" id="BtnNextPage" runat="server" onserverclick="BtnNextPage_Click">Próxima Página</button>
        </div>

        <%--LABEL INVISÍVEL PARA SELECIONAR O ID DO JOGO--%>
        <asp:HiddenField ID="IdJogoHiddenField"
            Value=""
            runat="server" />

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

    <script>
        function SetIdJogoHiddenField(id) {
            console.log(id);
            IdJogoHiddenField.value = id;
        }

        function BtnShowSelectedJogo(id) {
            $.ajax({
                type: "POST",
                url: "LojaJogos.aspx/BtnShowSelectedJogo",
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

        function ResultadoPesquisaJogo(filtro) {
            $.ajax({
                type: "POST",
                url: "LojaJogos.aspx/ResultadoPesquisaJogo",
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

        function BtnLoginCliente() {
            $.ajax({
                type: "POST",
                url: "LojaJogos.aspx/BtnLoginCliente",
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
                url: "LojaJogos.aspx/BtnLogoutCliente",
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

        /*DROPDOWN LOGIN-LOGOUT*/
        jQuery(document).ready(function (e) {
            function t(t) {
                e(t).bind("click", function (t) {
                    t.preventDefault();
                    e(this).parent().fadeOut()
                })
            }
            e(".dropdown-toggle").click(function () {
                var t = e(this).parents(".button-dropdown").children(".dropdown-menu").is(":hidden");
                e(".button-dropdown .dropdown-menu").hide();
                e(".button-dropdown .dropdown-toggle").removeClass("active");
                if (t) {
                    e(this).parents(".button-dropdown").children(".dropdown-menu").toggle().parents(".button-dropdown").children(".dropdown-toggle").addClass("active")
                }
            });
            e(document).bind("click", function (t) {
                var n = e(t.target);
                if (!n.parents().hasClass("button-dropdown")) e(".button-dropdown .dropdown-menu").hide();
            });

            e(document).bind("click", function (t) {
                var n = e(t.target);
                if (!n.parents().hasClass("button-dropdown")) e(".button-dropdown .dropdown-toggle").removeClass("active");
            })
        });
        /*DROPDOWN LOGIN-LOGOUT*/
    </script>
</body>
</html>
