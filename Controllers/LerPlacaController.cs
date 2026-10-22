using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
//using Newtonsoft.Json; // Install via NuGet: Newtonsoft.Json
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using VozDoPovo.Models;

namespace VozDoPovo.Controllers
{

    public class LerPlacaController : Controller
    {
        private const string ApiToken = "511bbb1efabebb18e7d019980a9b00c3ce85fac0";
        private const string ApiUrl = "https://api.platerecognizer.com/v1/plate-reader/";


        //[IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Enviar(IFormFile frame)
        {
            if (frame == null || frame.Length == 0)
            {
                return BadRequest(new
                {
                    mensagem = "Nenhum frame recebido."
                });
            }

            using var form = new MultipartFormDataContent();

            using var stream = frame.OpenReadStream();

            var fileContent = new StreamContent(stream);
            //fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(frame.ContentType);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            form.Add(fileContent, "upload", frame.FileName);
            form.Add(new StringContent("br"), "regions");

            //IA
            using (var client = new HttpClient())
            {
                // Add authentication header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", ApiToken);

                // Attach the image file
                var response = await client.PostAsync(ApiUrl, form);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API request failed: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }

                var jsonString = await response.Content.ReadAsStringAsync();

                JsonNode? node = JsonNode.Parse(jsonString);

                if (node?["results"] is JsonArray results && results.Count > 0 && results[0] is JsonNode result)
                    return Ok(new
                    {
                        placa = result["plate"]?.GetValue<string>(),
                        x0 = result["box"]?["xmin"]?.GetValue<int>(),
                        y0 = result["box"]?["ymin"]?.GetValue<int>(),
                        x1 = result["box"]?["xmax"]?.GetValue<int>(),
                        y1 = result["box"]?["ymax"]?.GetValue<int>()
                    });
                /*
                {
                  "processing_time": 24.019,
                  "results": [
                    {
                      "box": {
                        "xmin": 58,
                        "ymin": 138,
                        "xmax": 631,
                        "ymax": 359
                      },
                      "plate": "thi3e91",
                      "region": {
                        "code": "br",
                        "score": 0.663
                      },
                      "score": 0.999,
                      "candidates": [
                        {
                          "score": 0.999,
                          "plate": "thi3e91"
                        }
                      ],
                      "dscore": 0.832,
                      "vehicle": {
                        "score": 0,
                        "type": "Unknown",
                        "box": {
                          "xmin": 0,
                          "ymin": 0,
                          "xmax": 0,
                          "ymax": 0
                        }
                      }
                    }
                  ],
                  "filename": "1458_TSzcH_frame.jpg",
                  "version": 1,
                  "camera_id": null,
                  "timestamp": "2026-10-22T14:58:28.041844Z",
                  "image_width": 640,
                  "image_height": 480
                }
                */


            }


            return BadRequest();
        }
    }
}
