using System.ComponentModel.DataAnnotations;

namespace GymManagement_MVC_Project.DAL.Models.Enums;

public enum TrainerSpecialties
{
    [Display(Name = "General Fitness")]
    GeneralFitness = 1,
    Yoga = 2,
    Boxing = 3,
    CrossFit = 4,
    Cardio = 5,
    [Display(Name = "Strength Trainer")]
    StrengthTrainer = 6
}
