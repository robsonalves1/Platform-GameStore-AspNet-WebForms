<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComprarProduto.aspx.cs" Inherits="WebApplication2.View.ComprarProduto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Comprar produto</title>
    <link href="../Content/StyleSheet.css" rel="stylesheet" />
    <link href="../Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" style="color: white;" href="HomeLoja.aspx">Loja</a>
                <div id="DivSearchBar" class="d-flex" runat="server">
                    <input class="form-control mx-2" type="search" placeholder="Search" aria-label="search" id="InputSearchBar" autocomplete="off" runat="server" onkeyup="ShowListSearchProduct(this.value)" />
                    <div id="DivIconApagaResultados">
                        <button type="button" onclick="ShowListSearchProduct('')">
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

        <div class="container">
            <%--DIVALERT--%>
            <div id="DivAlert" runat="server" class="alert alert-danger position-absolute top-50 start-50 translate-middle" visible="false">
                <label id="LblAlert" runat="server"></label>
                <button type="button" class="btn-close" aria-label="Close" onclick="CloseDivAlert()"></button>
            </div>
            <%--DIVALERT AJAX--%>
            <div id="DivAlertAjax" runat="server" class="alert alert-danger position-absolute top-50 start-50 translate-middle">
                <label id="LabelAlertAjax" runat="server"></label>
                <button type="button" class="btn-close" aria-label="Close" onclick="CloseDivAlert()"></button>
            </div>
            <div class="row">
                <div class="col-12 pt-3">
                    <div class="row">
                        <div class="col-12 col-md-6">

                            <%--DIV SELECT MORE IMGS OF THE PRODUCTS--%>
                            <div id="updating-color-prototype">

                                <div id="main-image">
                                    <img id="imgs" src="https://n.nordstrommedia.com/ImageGallery/store/product/Zoom/8/_101474728.jpg?crop=pad&pad_color=FFF&format=jpeg&w=391&h=600&dpr=2&quality=60" runat="server" />
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

                        <%--PRODUCT--%>
                        <div class="col-12 col-md-6 ps-lg-10 p-3">
                            <!-- NAME -->
                            <h3 class="mb-2" id="CampoProdutoNome" runat="server">Nome</h3>

                            <!-- STARS / QUANTITY OF COMMENTS -->
                            <p>
                                <span class="mb-2" id="CampoProdutoNotaEstrelas" runat="server">5</span> <span id="CampoProdutoNotaQuantidadeDeComentarios" runat="server"></span>
                            </p>

                            <%--BRAND--%>
                            <h4 class="mb-2" id="CampoProdutoMarca" runat="server">Marca</h4>

                            <!-- PRICE -->
                            <div class="mb-7">
                                <span class="fs-lg fw-bold text-gray-350" id="CampoProdutoPreco" runat="server">$0.00</span>
                                <p>
                                    Quantidade: (<span class="fs-sm ms-1" id="CampoProdutoQuantidade" runat="server">(Em estoque)</span> Disponíveis)
                                </p>
                            </div>

                            <!-- SPECIFICATIONS -->
                            <div class="mb-7">
                                <div class="fs-lg text-gray-350 my-3" id="CampoCaracteristicas" runat="server"></div>
                            </div>

                            <div class="form-group">
                                <div class="row gx-5 mb-7">
                                    <%--BUY BUTTON--%>
                                    <div class="row">
                                        <button class="btn btn-secondary col-6" id="BtnCancelarCompra" runat="server" onserverclick="BtnCancelPurchase_Click">Voltar</button>
                                        <button type="button" class="btn btn-dark col-6" id="BtnComprarProduto" onclick="ComprarProduto()">
                                            Comprar
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--COMMENTS--%>
            <div class="row mt-5" id="DivComentarios">
                <h1>Comentários</h1>
                <div id="DivContainerAddComentarios" runat="server">
                    <div class="form-floating mb-2">
                        <textarea class="form-control mb-2" placeholder="Deixe seu comentário aqui" style="height: 100px" id="TxbComentarioProduto" runat="server"></textarea>
                        <label class="form-label">Comentário</label>
                    </div>
                    <div class="form-floating mb-2">
                        <input type="number" class="form-control" id="TxbNotaProduto" placeholder="Nota" runat="server" min="0" max="5" step="1" />
                        <label for="floatingNota">Nota</label>
                    </div>
                    <div class="form-floating mb-2">
                        <button type="button" class="btn btn-success" onclick="SendComment()" runat="server">Enviar</button>
                    </div>
                </div>

                <div id="DivAlertComentarios" runat="server" class="alert alert-danger mb-3 " visible="false">
                    <label id="LabelAlertComentarios" runat="server"></label>
                </div>

                <section class="mt-2 mb-2" id="SectionComentarios" runat="server">
                    <div class="px-5 mt-3">
                        <p>É um ótimo headset. O som de é de espantar, parece que qualquer música fica boa!</p>
                        <p>5</p>
                    </div>
                    <div class="px-5 mt-3">
                        <p>É um ótimo headset. O som de é de espantar, parece que qualquer música fica boa!</p>
                        <p>5</p>
                    </div>
                </section>
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
            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática</p>
        </footer>
    </form>

    <script src="../Scripts/jquery-3.6.4.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/JavaScriptComprarJogo.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>
</body>
</html>
