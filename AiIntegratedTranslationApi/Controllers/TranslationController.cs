using AiIntegratedTranslationApi.Models;
using AiIntegratedTranslationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiIntegratedTranslationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranslationController : Controller
{
    //GET: api/TranslateWithGpt
    [HttpGet("TranslateWithGpt")]
    [ProducesResponseType(typeof(AI_TranslationResponse_Json), StatusCodes.Status200OK)]
    public async Task<IActionResult> TranslateWithGpt(string fromLanguage, string toLanguage, string target, string gptApiKey)
    {
        try
        {
            var GptPesponse = await AI_Service.TranslateWithGptAsync(fromLanguage, toLanguage, target, gptApiKey);
            return Ok(GptPesponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //GET: api/TranslateWithOllama
    [HttpGet("TranslateWithOllama")]
    [ProducesResponseType(typeof(AI_TranslationResponse_Json), StatusCodes.Status200OK)]
    public async Task<IActionResult> TranslateWithOllama(string fromLanguage, string toLanguage, string target, string modelName)
    {
        try
        {
            AI_TranslationResponse_Json response = await AI_Service.TranslateWithOllamaAsync(fromLanguage, toLanguage, target, modelName);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //GET: api/OllamaModelList
    [HttpGet("GetOllamaModelList")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOllamaModelList()
    {
        try
        {
            var response = await AI_Service.GetOllamaModelListAsync();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
