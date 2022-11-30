using System.Web;
using MudBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;
using task5.Shared;
using task5;

namespace task5.Client.Pages
{

    public partial class Index : ComponentBase
    {
        [Inject]
        IJSRuntime JS { get; set; } = default!;
        
        [Inject]
        HttpClient httpClient { get; set; } = default!;
    }
}