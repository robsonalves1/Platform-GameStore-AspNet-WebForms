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
                    <input class="form-control mx-2" type="search" placeholder="Search" aria-label="search" id="InputSearchBar" autocomplete="off" runat="server" onkeyup="ShowListOfSearchedProduct(this.value)" />
                    <div id="DivIconApagaResultados">
                        <button type="button" onclick="ShowListOfSearchedProduct('')">
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
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaCadeirasGamer" runat="server" onserverclick="BtnGoToGamingChairPage_Click">Veja mais &raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/-monitor.png" />
                        <h2 class="fw-normal">Monitor Gamer</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaMonitores" runat="server" onserverclick="BtnGoToMonitorPage_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/hardware.jpg" />
                        <h2 class="fw-normal">Hardware</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaHardwares" runat="server" onserverclick="BtnGoToHardwarePage_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/--perifericos.png" />
                        <h2 class="fw-normal">Periféricos</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaPerifericos" runat="server" onserverclick="BtnGoToAccessoriesPage_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/-refrigeracao.png" />
                        <h2 class="fw-normal">Refrigeração</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaRefrigeracao" runat="server" onserverclick="BtnGoToCoolerPage_Click">Veja Mais&raquo;</button>
                        </p>
                    </div>
                    <!-- /.col-lg-4 -->
                    <div class="col-lg-4">
                        <img src="https://img.terabyteshop.com.br/dep-pop/gabinetes.png" />
                        <h2 class="fw-normal">Gabinetes</h2>
                        <p>
                            <button class="btn btn-secondary BtnDirecionaParaLojaInformatica" id="BtnDirecionaParaGabinetes" runat="server" onserverclick="BtnGoToCasePage_Click">Veja Mais&raquo;</button>
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
    <script src="../Scripts/JavaScriptHomeLoja.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>
</body>
</html>
