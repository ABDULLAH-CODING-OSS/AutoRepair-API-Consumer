using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ERP_Consumer.Helpers;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public abstract class BaseApiService<TDto, TCreate, TUpdate>
    : IApiService<TDto, TCreate, TUpdate>
    where TDto : class
    where TCreate : class
    where TUpdate : class
{
    protected readonly HttpClient _httpClient;
    protected readonly ILogger _logger;
    protected readonly string _endpoint;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected BaseApiService(HttpClient httpClient, ILogger logger, string endpoint)
    {
        _httpClient = httpClient;
        _logger = logger;
        _endpoint = endpoint;
    }

    public async Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(_endpoint);
            return await HandleCollectionResponse(response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Connection failure calling GET {Endpoint}", _endpoint);
            return ApiResponse<IEnumerable<TDto>>.Fail("Unable to connect to the ERP service. Please try again later.", 503);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout calling GET {Endpoint}", _endpoint);
            return ApiResponse<IEnumerable<TDto>>.Fail("The request timed out. Please try again.", 408);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling GET {Endpoint}", _endpoint);
            return ApiResponse<IEnumerable<TDto>>.Fail("An unexpected error occurred.", 500);
        }
    }

    public async Task<ApiResponse<TDto>> GetByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
            return await HandleSingleResponse(response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Connection failure calling GET {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("Unable to connect to the ERP service.", 503);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout calling GET {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("The request timed out.", 408);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling GET {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("An unexpected error occurred.", 500);
        }
    }

    public async Task<ApiResponse<TDto>> CreateAsync(TCreate dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, dto, _jsonOptions);
            return await HandleSingleResponse(response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Connection failure calling POST {Endpoint}", _endpoint);
            return ApiResponse<TDto>.Fail("Unable to connect to the ERP service.", 503);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout calling POST {Endpoint}", _endpoint);
            return ApiResponse<TDto>.Fail("The request timed out.", 408);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling POST {Endpoint}", _endpoint);
            return ApiResponse<TDto>.Fail("An unexpected error occurred.", 500);
        }
    }

    public async Task<ApiResponse<TDto>> UpdateAsync(int id, TUpdate dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{id}", dto, _jsonOptions);
            return await HandleSingleResponse(response);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Connection failure calling PUT {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("Unable to connect to the ERP service.", 503);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout calling PUT {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("The request timed out.", 408);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling PUT {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<TDto>.Fail("An unexpected error occurred.", 500);
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");

            if (response.IsSuccessStatusCode)
                return ApiResponse<bool>.Ok(true);

            var errorMessage = await ExtractErrorMessage(response);
            _logger.LogWarning("DELETE {Endpoint}/{Id} returned {StatusCode}: {Error}", _endpoint, id, (int)response.StatusCode, errorMessage);
            return ApiResponse<bool>.Fail(errorMessage, (int)response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Connection failure calling DELETE {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<bool>.Fail("Unable to connect to the ERP service.", 503);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout calling DELETE {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<bool>.Fail("The request timed out.", 408);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error calling DELETE {Endpoint}/{Id}", _endpoint, id);
            return ApiResponse<bool>.Fail("An unexpected error occurred.", 500);
        }
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private async Task<ApiResponse<TDto>> HandleSingleResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0)
                return ApiResponse<TDto>.Ok(default!);

            var data = await response.Content.ReadFromJsonAsync<TDto>(_jsonOptions);
            if (data is null)
                return ApiResponse<TDto>.Ok(default!);
            return ApiResponse<TDto>.Ok(data);
        }

        var error = await ExtractErrorMessage(response);
        _logger.LogWarning("{Method} {Endpoint} returned {StatusCode}: {Error}",
            response.RequestMessage?.Method, response.RequestMessage?.RequestUri,
            (int)response.StatusCode, error);

        return ApiResponse<TDto>.Fail(FriendlyMessage(response.StatusCode, error), (int)response.StatusCode);
    }

    private async Task<ApiResponse<IEnumerable<TDto>>> HandleCollectionResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<IEnumerable<TDto>>(_jsonOptions);
            return ApiResponse<IEnumerable<TDto>>.Ok(data ?? Enumerable.Empty<TDto>());
        }

        var error = await ExtractErrorMessage(response);
        _logger.LogWarning("GET {Endpoint} returned {StatusCode}: {Error}", _endpoint, (int)response.StatusCode, error);
        return ApiResponse<IEnumerable<TDto>>.Fail(FriendlyMessage(response.StatusCode, error), (int)response.StatusCode);
    }

    private static async Task<string> ExtractErrorMessage(HttpResponseMessage response)
    {
        try
        {
            var body = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body))
                return response.ReasonPhrase ?? "Unknown error.";

            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? body;
            if (doc.RootElement.TryGetProperty("message", out var message))
                return message.GetString() ?? body;
            if (doc.RootElement.TryGetProperty("error", out var err))
                return err.GetString() ?? body;

            return body.Length > 300 ? body[..300] : body;
        }
        catch
        {
            return response.ReasonPhrase ?? "Unknown error.";
        }
    }

    private static string FriendlyMessage(HttpStatusCode code, string detail) => code switch
    {
        HttpStatusCode.BadRequest => $"Validation error: {detail}",
        HttpStatusCode.NotFound => "The requested record was not found.",
        HttpStatusCode.Conflict => $"Conflict: {detail}",
        HttpStatusCode.InternalServerError => "The server encountered an error. Please try again later.",
        HttpStatusCode.ServiceUnavailable => "The ERP service is currently unavailable.",
        _ => detail
    };
}
