<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpCliente.aspx.cs" Inherits="WebApplication2.View.SignUpCliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <link href="../Content/StyleSheetLj.css" rel="stylesheet" />
    <title>Cadastro cliente</title>
</head>
<body>
    <form runat="server">
        <nav id="MainNavBar" class="navbar p-3">
            <div class="container-fluid">
                <a class="navbar-brand" href="HomeLoja.aspx" style="color: white;">Loja</a>
            </div>
        </nav>
        <main class="form-signin w-50 mx-auto mt-5">
            <h1 class="h3 mb-3 fw-normal">Cadastrar</h1>

            <div class="form-floating mb-3">
                <input type="text" class="form-control" id="TxbNome" placeholder="Nome" runat="server" />
                <label>Nome</label>
            </div>

            <div class="form-floating mb-3">
                <input type="email" class="form-control" id="TxbEmail" placeholder="name@example.com" runat="server" />
                <label>Email</label>
            </div>
            <div class="form-floating mb-3">
                <input type="password" class="form-control" id="TxbSenha" placeholder="Senha" runat="server" />
                <label>Senha</label>
            </div>

            <div class="form-floating mb-3">
                <a href="LoginCliente.aspx">Já é cadastrado?</a>
            </div>

            <div id="DivAlert" runat="server" class="alert alert-danger mb-3 " visible="false">
                <label id="LblAlert" runat="server"></label>
            </div>

            <div id="divAlertUsuarioCadastrado" runat="server" class="alert alert-success mb-3 " visible="false">
                Cliente cadastrado com sucesso!
            </div>

            <div class="form-floating">
                <div class="row">
                    <button type="button" id="BtnCancelarCadastrarCliente" class="col-6  btn btn-lg btn-secondary" runat="server" onserverclick="BtnCancelSignUpClient_Click">Voltar</button>
                    <button type="submit" id="BtnCadastrarCLiente" class="col-6 btn btn-lg btn-primary" runat="server" onserverclick="BtnSignUpClient_Click">Cadastrar</button>
                </div>
            </div>
        </main>

        <!-- FOOTER -->
        <footer class="container text-center">
            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática e Jogos</p>
        </footer>
    </form>
</body>
</html>
