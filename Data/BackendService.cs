using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Frontend.Data
{
    public class BackendService
    {
        private static readonly HttpClient connect = new HttpClient();

        public async Task<String> GetStatus()
        {
            connect.DefaultRequestHeaders.Accept.Clear();
            var stringTask = connect.GetStringAsync("https://cherrypy-backend.herokuapp.com/");
            Console.Out.WriteLine(stringTask.Result);
            return stringTask.Result;

        }

        public async Task<MathQuestion> GetQuestion()
        {
            connect.DefaultRequestHeaders.Accept.Clear();
            var resQuestion = connect.GetStringAsync("https://cherrypy-backend.herokuapp.com/api/calculator");
            MathQuestion response = JsonSerializer.Deserialize<MathQuestion>(resQuestion.Result);
            return response;
        }

        public async Task<MathQuestion> PostAnswer(string number, string questionNumber)
        {
            // solution for posting data with correct length(to fix http 411 error)
            // https://stackoverflow.com/questions/52387065/asp-net-core-httpclient-gets-response-411-length-required-error

            connect.DefaultRequestHeaders.Accept.Clear();
            var jsonRequest = new { questionNumber = questionNumber , answer = number };
            var jsonString = JsonConvert.SerializeObject(jsonRequest);
            var contentRequest = new StringContent(jsonString, Encoding.UTF8, "application/json");
            contentRequest.Headers.ContentLength = Encoding.UTF8.GetByteCount(jsonString);
            //Console.WriteLine(contentRequest.Headers.ToString());

            var resQuestion = await connect.PostAsync("https://cherrypy-backend.herokuapp.com/api/calculator", contentRequest);
            //MyClass response = JsonSerializer.Deserialize<MyClass>(resQuestion.Result.Headers.GetValues("status"));
            var result = await resQuestion.Content.ReadAsStringAsync();
            var resMathQuestion = JsonSerializer.Deserialize<MathQuestion>(result);
            Console.WriteLine(resMathQuestion.message);
            return resMathQuestion;
        }
    }
}