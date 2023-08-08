<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="WebApplication2.View.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastrar</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="text-center">
    <main class="form-signin w-50 mx-auto mt-5">
        <form runat="server">
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
                <input type="text" class="form-control" id="TxbCargo" placeholder="Administrador" runat="server" />
                <label>Cargo</label>
            </div>

            <div class="form-floating mb-3">
                <select class="form-select" aria-label="Default select example" id="SelectDepartamento" runat="server">
                    <option selected="selected">Selecione o departamento</option>
                    <option value="Diretoria">Diretoria</option>
                    <option value="Financas">Finanças</option>
                    <option value="Comercial">Comercial</option>
                    <option value="Marketing">Marketing</option>
                    <option value="RH">RH</option>
                </select>
            </div>

            <div class="form-floating mb-3">
                <select class="form-select" aria-label="Default select example" id="SelectAdministrador" runat="server">
                    <option selected="selected">Selecione opção</option>
                    <option value="true">Sim</option>
                    <option value="false">Não</option>
                </select>
            </div>
             
            <div class="form-floating mb-3">
                <a href="Login.aspx">Já é cadastrado?</a>
            </div>

            <div id="divAlert" runat="server" class="alert alert-danger mb-3 " visible="false">
                <label id="lblAlert" runat="server"></label>
            </div>

            <div id="divAlertUsuarioCadastrado" runat="server" class="alert alert-success mb-3" visible="false">
                Usuário cadastrado com sucesso!
            </div>

            <div class="form-floating">
                <div class="row">
                    <button type="button" id="BtnCancelarCadastrarUsuario" class="col-6  btn btn-lg btn-secondary" runat="server" onserverclick="BtnCancelSignUpUser_Click">Voltar</button>
                    <button type="submit" id="Button1" class="col-6 btn btn-lg btn-primary" runat="server" onserverclick="BtnSignUpUser_Click">Cadastrar</button>
                </div>
            </div>

            <p class="mt-5 mb-3 text-body-secondary">© 2023 Loja de Informática</p>
        </form>
    </main>
</body>
</html>
