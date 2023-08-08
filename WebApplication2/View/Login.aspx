<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication2.View.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="text-center">
    <main class="form-signin w-50 mx-auto mt-5">
        <form runat="server">
            <h1 class="h3 mb-3 fw-normal">Entrar</h1>

            <div class="form-floating mb-3">
                <input type="email" class="form-control" id="TxbEmail" placeholder="name@example.com" runat="server" />
                <label for="floatingInput">Email</label>
            </div>
            <div class="form-floating mb-3">
                <input type="password" class="form-control" id="TxbSenha" placeholder="Senha" runat="server"/>
                <label for="floatingPassword">Senha</label>
            </div>

            <div class="form-floating mb-3">
                <a href="SignUp.aspx">Não é cadastrado?</a>
            </div>

            <div id="divAlert" runat="server" class="alert alert-danger mb-3 " visible="false">
                <label id="lblAlert" runat="server"></label>
            </div>

            <button class="w-100 btn btn-lg btn-primary" id="BtnLogin" runat="server" onserverclick="BtnLogin_Click">Entrar</button>
            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática</p>
        </form>
    </main>
</body>
</html>
