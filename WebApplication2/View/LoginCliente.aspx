<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginCliente.aspx.cs" Inherits="WebApplication2.View.LoginCliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <title>Login cliente</title>
</head>
<body>

    <form runat="server">
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" href="HomeLoja.aspx" style="color: white;">Loja</a>
            </div>
        </nav>

        <main class="form-signin w-50 mx-auto mt-5">
            <h1 class="h3 mb-3 fw-normal">Entrar</h1>

            <div class="form-floating mb-3">
                <input type="email" class="form-control" id="TxbEmail" placeholder="name@example.com" runat="server" />
                <label for="floatingInput">Email</label>
            </div>
            <div class="form-floating mb-3">
                <input type="password" class="form-control" id="TxbSenha" placeholder="Senha" runat="server" />
                <label for="floatingPassword">Senha</label>
            </div>

            <div class="form-floating mb-3">
                <a href="SignUpCliente.aspx">Não é cadastrado?</a>
            </div>

            <div id="DivAlert" runat="server" class="alert alert-danger mb-3 " visible="false">
                <label id="LblAlert" runat="server"></label>
            </div>

            <button class="w-100 btn btn-lg btn-primary" id="BtnLogin" runat="server" onserverclick="BtnLoginClient_Click">Entrar</button>
        </main>

        <!-- FOOTER -->
        <footer class="container text-center">
            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática e Jogos</p>
        </footer>
    </form>
</body>
</html>
