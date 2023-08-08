<%@  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="HomeLoja.aspx.cs" Inherits="WebApplication2.View.HomeLoja" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <title>Home Loja Gamer</title>
</head>
<body>
    <form id="HomeLoja" runat="server">

        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" style="color: white;">Loja</a>
                <div id="DivSearchBar" class="d-flex" runat="server">
                    <input class="form-control mx-2" type="search" placeholder="Search" aria-label="search" id="InputSearchBar" autocomplete="off" runat="server" onkeyup="ResultadoPesquisaProduto(this.value)" />
                    <div id="DivIconApagaResultados">
                        <button type="button" onclick="ResultadoPesquisaProduto('')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                                <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                                <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z" />
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

        <div id="DivResultados">
        </div>

        <main>

            <div id="myCarousel" class="carousel slide mb-6" data-bs-ride="carousel" data-bs-theme="light">
                <div class="carousel-indicators">
                    <button type="button" data-bs-target="#myCarousel" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                    <button type="button" data-bs-target="#myCarousel" data-bs-slide-to="1" aria-label="Slide 2"></button>
                </div>
                <div class="carousel-inner" style="text-align: center">

                    <div class="carousel-item active">
                        <img src="../Img/1.png" />
                    </div>
                    <div class="carousel-item">
                        <img src="../Img/2.png" />
                    </div>
                </div>
                <button class="carousel-control-prev" type="button" data-bs-target="#myCarousel" data-bs-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Previous</span>
                </button>
                <button class="carousel-control-next" type="button" data-bs-target="#myCarousel" data-bs-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="visually-hidden">Next</span>
                </button>
            </div>


            <div class="container marketing mt-3 text-center">
                <div class="row">
                    <div id="DivAlert" runat="server">
                        <label id="LblAlert" runat="server"></label>
                    </div>
                </div>


                <!-- LOJA DE INFORMÁTICA -->
                <hr />
                <h1 class="mt-5 mb-5">Loja de Informática</h1>
                <div class="row pb-5">
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/cadeira-gamer.jpg" />
                        <h2 class="fw-normal">Cadeiras Gamer</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaCadeirasGamer" runat="server" onserverclick="BtnDirecionaParaCadeirasGamer_Click">Veja mais &raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/-monitor.png" />
                        <h2 class="fw-normal">Monitor Gamer</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaMonitores" runat="server" onserverclick="BtnDirecionaParaMonitores_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/hardware.jpg" />
                        <h2 class="fw-normal">Hardware</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaHardwares" runat="server" onserverclick="BtnDirecionaParaHardwares_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/--perifericos.png" />
                        <h2 class="fw-normal">Periféricos</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaPerifericos" runat="server" onserverclick="BtnDirecionaParaPerifericos_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/-refrigeracao.png" />
                        <h2 class="fw-normal">Refrigeração</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaRefrigeracao" runat="server" onserverclick="BtnDirecionaParaRefrigeracao_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/gabinetes.png" />
                        <h2 class="fw-normal">Gabinetes</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaGabinetes" runat="server" onserverclick="BtnDirecionaParaGabinetes_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                </div>
                <!-- /.row -->

                <hr />

                <%--LOJA DE JOGOS--%>
                <h1 class="mt-5 mb-5">Loja de Jogos</h1>
                <div id="DivLojaJogos" class="row pb-5" runat="server">
                </div>

                <hr />

                <%--LISTA DE DESENVOLVEDORAS DE JOGOS--%>
                <div id="DivDesenvolvedorasDeJogo" class="mt-5 mb-5 pt-3" runat="server">
                </div>
            </div>
            <!-- /.container -->

            <!-- FOOTER -->
            <footer class="container text-center mt-4">
                <p class="float-end">
                    <a href="#">
                        <svg xmlns="http://www.w3.org/2000/svg" width="25" height="25" fill="currentColor" class="bi bi-chevron-double-up" viewBox="0 0 16 16">
                            <path fill-rule="evenodd" d="M7.646 2.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 3.707 2.354 9.354a.5.5 0 1 1-.708-.708l6-6z" />
                            <path fill-rule="evenodd" d="M7.646 6.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 7.707l-5.646 5.647a.5.5 0 0 1-.708-.708l6-6z" />
                        </svg>
                    </a>
                </p>
                <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática e Jogos</p>
            </footer>
        </main>



    </form>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.6.4.min.js"></script>

    <script>
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

        function BtnLoginCliente() {
            $.ajax({
                type: "POST",
                url: "HomeLoja.aspx/BtnLoginCliente",
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
                url: "HomeLoja.aspx/BtnLogoutCliente",
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

        function ResultadoPesquisaProduto(filtro) {
            $.ajax({
                type: "POST",
                url: "Loja.aspx/ResultadoPesquisaProduto",
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

        function LimpaResultadoPesquisaProduto() {
            $.ajax({
                type: "POST",
                url: "Loja.aspx/LimpaResultadoPesquisaProduto",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById("DivResultados").style.visibility = 'hidden';
                    document.getElementById("DivResultados").innerHTML = "";
                    console.log(response.d);
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }

        function BtnShowJogos() {
            $.ajax({
                type: "POST",
                url: "HomeLoja.aspx/BtnShowJogos",
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


        function BtnShowDesenvolvedoras() {
            $.ajax({
                type: "POST",
                url: "HomeLoja.aspx/BtnShowDesenvolvedoras",
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
