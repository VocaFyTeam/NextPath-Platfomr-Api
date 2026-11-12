using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

namespace NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;

public static class CareerResourceFromEntityAssembler
{
    public static CareerResource ToResourceFromEntity(Career entity)
    {
        return new CareerResource(
            entity.Id,
            entity.Name,
            entity.Emoji,
            entity.Area,
            entity.ImageUrl,
            entity.ShortDescription,
            entity.Description,
            entity.DurationYears,
            entity.DurationLabel,
            entity.Semesters,
            entity.Modality,
            entity.ModalityDetail,
            entity.Universities.Select(u => new UniversityResource(u.Name, u.Acronym, u.StudyPlanUrl)),
            entity.FieldOfWork,
            entity.MainField,
            entity.Skills,
            new SalaryRangeResource(entity.AverageSalary.Min, entity.AverageSalary.Max),
            new SalaryRangeResource(entity.SalaryRange.Min, entity.SalaryRange.Max),
            entity.EmployabilityRate,
            entity.DemandLevel,
            entity.BaseCompatibility,
            new RiasecProfileResource(entity.Profile.Artistic, entity.Profile.Social, entity.Profile.Investigative,
                entity.Profile.Enterprising, entity.Profile.Conventional),
            entity.Projection.Select(p => new ProjectionStageResource(p.Period, p.Role, p.Salary)),
            entity.Recommendations);
    }
}
