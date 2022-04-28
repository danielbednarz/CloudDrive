using System.ComponentModel.DataAnnotations;

namespace CloudDrive.Dictionaries
{
    public enum OperationType
    {
        [Display(Name = "Dodawanie")]
        Add = 0,
        [Display(Name = "Aktualizacja")]
        Update = 1,
        [Display(Name = "Usuwanie")]
        Remove = 2
    }
}
