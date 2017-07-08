using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SqlInjection.Controllers
{
    public class ContaController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string senha)
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProdutosDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //var consulta = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = '" + usuario + "' AND Senha = '" + senha + "'";
            var consulta = "SELECT Id " +
                                "FROM Usuarios " +
                                "WHERE " +
                                    "Usuario = @usuario AND " +
                                    "Senha = @senha;";

            try
            {
                using (var conexao = new SqlConnection(connectionString))
                {
                    conexao.Open();

                    using (SqlCommand comando = new SqlCommand(consulta, conexao))
                    {
                        // Passando os parâmtros de SQL
                        comando.Parameters.Add(new SqlParameter("@usuario", usuario));
                        comando.Parameters.Add(new SqlParameter("@senha", senha));

                        var resultado = (int)comando.ExecuteScalar();
                        if (resultado > 0)
                        {
                            //Cookie
                            var cookie = new HttpCookie("idUsuario", resultado.ToString());
                            Response.Cookies.Add(cookie);

                            //Acessando e alterando o cookie via Chrome
                            /* 
                                1. F12
                                2. Console
                                3. Document.Cookie
                                4. Document.Cookie = "idUsuario=2"
                             */

                            ViewBag.Mensagem = "Login efetuado com sucesso";
                        }
                        else
                            ViewBag.Mensagem = "Falha no login";
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Mensagem = "Erro: " + e.Message;
            }

            return View();

        }
    }
}