using Domain.Enities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BibliotecaApi.Services
{
    public class CepService
    {
        public async Task<Adress> BuscarEnderecosAsync(string cep)
        {
            try
            {
                var httpClient = new HttpClient();
                string url = String.Format("https://viacep.com.br/ws/{0}/json/", cep);
                var jsonOptions = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                };

                var res = await httpClient.GetAsync(url);


                if ((int)res.StatusCode == 404)
                {
                    Console.WriteLine("Ocoreu uma bad Resquest");
                }
                else if ((int)res.StatusCode == 500)
                {
                    Console.WriteLine("Ocorreu um erro no servidor");
                }

                string resposta = await res.Content.ReadAsStringAsync();
                var endereco = JsonConvert.DeserializeObject<Adress>(resposta);

                if (endereco.Cep is not null)
                    return endereco;
                else
                    return null;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Cep Invalido");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Problema na comunicação entre sistemas");
            }
        }
    }
}
