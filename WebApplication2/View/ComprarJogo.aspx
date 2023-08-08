<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="ComprarJogo.aspx.cs" Inherits="WebApplication2.View.ComprarJogo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheet.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLojaJogos.css" rel="stylesheet" />
    <title>Comprar Jogo</title>
</head>
<body>
    <form id="form1" runat="server">
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

        <div class="alert alert-danger position-absolute top-50 start-50 translate-middle" role="alert" id="DivAlert" runat="server" visible="false">
            <label id="LblAlert" runat="server"></label>
        </div>
        <div id="DivSelectedJogo" runat="server">
            <div class="container">
                <%--DIVALERT EXIBE ERROS--%>
                <div id="Div1" runat="server" class="alert alert-danger position-absolute top-50 start-0" visible="false">
                    <label id="Label1" runat="server"></label>
                </div>
                <div class="row">
                    <div class="col-12 pt-3">
                        <div class="row">
                            <div class="col-12 col-md-6">

                                <div id="updating-color-prototype" class="text-center">

                                    <div id="main-image">
                                        <%--IMAGEM DO JOGO--%>
                                        <img id="imgs" src="https://n.60" runat="server" />
                                    </div>


                                    <div id="SwatchesSection" runat="server">
                                    </div>

                                    <div id="button" onclick="handleTap()">
                                        <div id="sneaky-spacer">
                                            <p id="updating-text">Updating color...</p>
                                        </div>

                                        <div id="transformer">
                                            <p id="button-text">Update color</p>
                                            <div id="spinner-wrapper">
                                                <?xml version="1.0" encoding="UTF-8" ?>
                                                <svg id="not-spinning" width="37px" height="37px" viewBox="0 0 37 37" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
                                                    <defs>
                                                        <linearGradient x1="50%" y1="0%" x2="50%" y2="100%" id="linearGradient-1">
                                                            <stop stop-color="#FFFFFF" stop-opacity="0" offset="0%"></stop>
                                                            <stop stop-color="#979797" offset="100%"></stop>
                                                        </linearGradient>
                                                    </defs>
                                                    <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd" stroke-dasharray="55" stroke-linecap="round">
                                                        <g stroke="url(#linearGradient-1)">
                                                            <circle id="Oval" cx="18.5" cy="18.5" r="17.5"></circle>
                                                        </g>
                                                    </g>
                                                </svg>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div id="DivNameAndDescription" class="col-12 col-md-6 ps-lg-10 p-3">

                                <!-- NOME DO JOGO -->
                                <h3 class="mb-2" id="FieldNameJogo" runat="server">Nome</h3>

                                <%--DESCRIÇÃO DO JOGO--%>
                                <p id="FieldDescriptionJogo" runat="server"></p>

                            </div>
                            <div id="DivDetailsJogo" class="row mt-3">
                                <%--METACRITIC DO JOGO--%>
                                <div class="col-2">
                                    <span>Metacritic:</span>
                                    <p id="FieldMetacriticJogo" runat="server"></p>
                                </div>
                                <%--RATING DO JOGO--%>
                                <div class="col-2">
                                    <span>Nota:</span>
                                    <p id="FieldRatingJogo" runat="server"></p>
                                </div>
                                <%--RELEASED DO JOGO--%>
                                <div class="col-2">
                                    <span>Lançamento:</span>
                                    <p id="FieldReleasedJogo" runat="server"></p>
                                </div>
                                <%--QUANTIDADE COMPRADA--%>
                                <div class="col-2">
                                    <span>Quantidade adquirida:</span>
                                    <p id="FieldQuantityPurchased" runat="server">0</p>
                                </div>
                                <div class="col-4">
                                    <button type="button" class="btn btn-success mb-2" id="BtnBuyJogo" data-bs-toggle="modal" data-bs-target="#DivPurchaseDecision">Comprar</button>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <!-- MODAL PURCHASE SUCCESSFULLY MADE! -->
                        <div class="modal fade" id="DivPurchaseDecision" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h1 class="modal-title fs-5">Modal title</h1>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body" id="DivBodyPurchaseDecision" runat="server">
                                        Por favor, faça login para comprar jogo.
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Cancelar</button>
                                        <button type="button" class="btn btn-primary" id="BtnConfirmPurchase" onserverclick="BtnConfirmPurchase_Click" visible="false" runat="server">Sim</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- MODAL PURCHASE SUCCESSFULLY MADE! -->
                        <div class="modal fade" id="DivPurchaseSuccess" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h1 class="modal-title fs-5" id="exampleModalLabel">Modal title</h1>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        Compra realizada com sucesso!
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Fechar</button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--DIV TRAILERS DO JOGO--%>
                        <div id="DivTrailerMovies" class="mt-4" runat="server" visible="false">
                        </div>
                        <%--DIV ACHIEVEMENTS--%>
                        <div id="DivAchievements" class="my-5" runat="server" visible="false">
                        </div>
                        <%--DIV JOGOS DA MESMA SÉRIE--%>
                        <div id="DivGameSameSerie" class="my-5" runat="server" visible="false">
                        </div>
                        <%--DIV TIME DE DESENVOLVEDORES--%>
                        <div id="DivDevelopmentTeam" class="mt-5" runat="server" visible="false">
                        </div>
                    </div>
                </div>
            </div>
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
    <script src="../Scripts/ComprarProdutoJS.js"></script>

    <script>
        function BtnShowSelectedJogo(id) {
            $.ajax({
                type: "POST",
                url: "ComprarJogo.aspx/BtnShowSelectedJogo",
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
                url: "ComprarJogo.aspx/BtnLoginCliente",
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
                url: "ComprarJogo.aspx/BtnLogoutCliente",
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
