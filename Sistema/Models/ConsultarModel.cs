using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class ConsultarModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Preencha o campo Nome Fantasia")]
        public string NomeFantasia { get; set; }
        public string CNPJ_CPF { get; set; }

        public static List<ConsultarModel> RecuperarLista()
        {
            var ret = new List<ConsultarModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from tbPessoa order by NOME_FANTASIA";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ConsultarModel
                        {
                            Id = (int)reader["PESSOA_ID"],
                            NomeFantasia = (string)reader["NOME_FANTASIA"],
                            CNPJ_CPF = (string)reader["CNPJ_CPF"]
                        });
                    }
                }
            }

            return ret;
        }

        public static ConsultarModel RecuperarPeloId(int id)
        {
            ConsultarModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from tbPessoa where (PESSOA_ID = {0})", id);
                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new ConsultarModel
                        {
                            Id = (int)reader["PESSOA_ID"],
                            NomeFantasia = (string)reader["NOME_FANTASIA"],
                            CNPJ_CPF = (string)reader["CNPJ_CPF"]
                        };
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model != null)
                    {
                        comando.CommandText = string.Format(
                            "update tbPessoa set NOME_FANTASIA='{1}' where PESSOA_ID = {0}",
                            this.Id, this.NomeFantasia);
                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }
            return ret;
        }
    }
}