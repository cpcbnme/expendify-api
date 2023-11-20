namespace FinancialApplication.Controllers;

[ApiVersion("1.0")]
[Route("api/v{v:apiversion}/categories")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly IRepositoryServiceManager _repo;
    private readonly ILogger<CategoryController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoryController(IRepositoryServiceManager repo, ILogger<CategoryController> logger, UserManager<ApplicationUser> userManager)
    {
        _repo = repo;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet(CategoryRoutes._getAllCategories)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _repo.CategoryService.GetAll();
        return StatusCode(StatusCodes.Status200OK, new ApiResponse<IEnumerable<CategoryDTO>>()
        {
            statusCode = StatusCodes.Status200OK,
            message = "Categories retrieved successfully",
            data = categories,
            hasError = false
        });
    }

    [HttpPost(CategoryRoutes._createNewCategory)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNewCategory(CategoryCreateDTO model)
    {
        if (model is null) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Request body cannot be empty",
            data = null
        });

        if (string.IsNullOrWhiteSpace(model.Title)) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Title is required",
            data = null
        });

        if(string.IsNullOrWhiteSpace(model.Description)) return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>()
        {
            statusCode = StatusCodes.Status400BadRequest,
            hasError = true,
            message = "Description is required",
            data = null
        });
        await _repo.CategoryService.Add(model);
        return StatusCode(StatusCodes.Status201Created, new ApiResponse<CategoryDTO>()
        {
            statusCode = StatusCodes.Status201Created,
            hasError = false,
            message = "Category created successfully",
            data = null
        });
    }

    #region Helpers
    private async Task<ApplicationUser> GetUser()
    {
        var username = User.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);
        return user;
    }
    #endregion
}
