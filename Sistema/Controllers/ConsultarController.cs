using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class ConsultarController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View(ConsultarModel.RecuperarLista());
        }

        [HttpPost]
        [Authorize]
        public ActionResult RecuperarValores(int Id)
        {
            return Json(ConsultarModel.RecuperarPeloId(Id));
        }

        [HttpPost]
        [Authorize]
        public ActionResult SalvarValores(ConsultarModel model)
        {
            var resultado = "OK";
            var mensagem = new List<String>();
            var IdSalvo = string.Empty;

            if (!ModelState.IsValid)
            {
                resultado = "Aviso";
                mensagem = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).ToList();
            }
            else
            {
                try
                {
                    var id = model.Salvar();
                    if (id > 0)
                    {
                        IdSalvo = id.ToString();
                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {
                    resultado = "ERRO";
                }
            }

            return Json(new { Resultado = resultado, Mensagem = mensagem, IdSalvo = IdSalvo});
        }
    }
}