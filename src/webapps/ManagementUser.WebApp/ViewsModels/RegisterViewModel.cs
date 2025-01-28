using System.ComponentModel.DataAnnotations;

namespace ManagementUser.WebApp.ViewsModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    [MinLength(4, ErrorMessage = "A senha deve ter pelo menos 4 caracteres.")]
    public string Password { get; set; }
    
}