<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loja.aspx.cs" Inherits="WebApplication2.View.Loja" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <title>Produtos à venda</title>
</head>
<body>
    <form id="form1" runat="server">
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" style="color: white;" href="HomeLoja.aspx">Loja</a>
                <div id="DivSearchBar" class="d-flex" runat="server">
                    <input class="form-control mx-2" type="search" placeholder="Search" aria-label="search" id="InputSearchBar" autocomplete="off" runat="server" onkeyup="ResultSearchedProduct(this.value)" />
                    <div id="DivIconApagaResultados">
                        <button type="button" onclick="ResultSearchedProduct('')">
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

        <div>
            <main>

                <div class="album py-5 bg-body-tertiary">
                    <div class="row">
                        <div class="col-2">
                            <div id="DivFiltros" runat="server">
                                <section>
                                    <h1>Preço</h1>
                                    <button runat="server" type="button" id="BtnLimparFiltro" onserverclick="BtnClearFilter_Click">Limpar</button>
                                    <button runat="server" type="button" id="BtnProdutosAte100" onserverclick="BtnFilterProducts_Click" value="WHERE produtoPreco <= 100">até R$100</button>
                                    <button runat="server" type="button" id="BtnProdutos100Ate500" onserverclick="BtnFilterProducts_Click" value="WHERE produtoPreco BETWEEN 100 AND 500">de R$100 a R$500</button>
                                    <button runat="server" type="button" id="BtnProdutos500Ate1000" onserverclick="BtnFilterProducts_Click" value="WHERE produtoPreco BETWEEN 500 AND 1000">de R$500 a R$1000</button>
                                    <button runat="server" type="button" id="BtnProdutosAcima1000" onserverclick="BtnFilterProducts_Click" value="WHERE produtoPreco >= 1000">mais R$1000</button>
                                </section>
                            </div>
                        </div>
                        <div class="col-10">
                            <div class="container">
                                <div id="DivAlert" runat="server" class="alert alert-danger position-absolute top-50 start-0" visible="false">
                                    <label id="LblAlert" runat="server"></label>
                                </div>
                                <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-3" id="DivTodosProdutos" runat="server">
                                </div>
                            </div>
                        </div>
                    </div>


                </div>

            </main>
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

    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/jquery-3.6.4.min.js"></script>
    <script src="../Scripts/JavaScriptLoja.js"></script>
    <script src="../Scripts/JavaScriptDropdownLoginLogout.js"></script>
</body>
</html>
