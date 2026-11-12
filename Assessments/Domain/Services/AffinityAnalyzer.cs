using System.Globalization;
using System.Text;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Services;

/// <summary>
///     Domain service that calculates the affinity profile from the answers.
///     It is the same algorithm the frontend had in affinity-analyzer.js, now on the server.
/// </summary>
public static class AffinityAnalyzer
{
    private const int OpenQuestionAreaWeight = 1;
    private const int ChoiceOptionWeight = 2;
    private const int MinScore = 20;

    private static readonly Dictionary<EAffinityDimension, string[]> Keywords = new()
    {
        [EAffinityDimension.Artistic] =
        [
            "dibuj", "disen", "pint", "arte", "artist", "music", "crea", "foto", "video", "escrib", "color",
            "ilustr", "imagin", "cine", "moda", "decor", "esteti", "visual", "animac", "tocar", "cant", "baile",
            "teatro"
        ],
        [EAffinityDimension.Social] =
        [
            "ayud", "ensen", "amig", "escuch", "equipo", "person", "comunidad", "volunt", "compart", "cuid",
            "acompan", "conversa", "grupo", "social", "nino", "famil", "apoy", "emocion", "empat"
        ],
        [EAffinityDimension.Investigative] =
        [
            "investig", "cienci", "analiz", "matemat", "logic", "program", "experiment", "descubr", "estudi",
            "resolver", "problema", "datos", "tecnolog", "fisic", "quimic", "biolog", "curios", "leer", "computad"
        ],
        [EAffinityDimension.Enterprising] =
        [
            "lider", "negocio", "vend", "empres", "emprend", "dirig", "organiz", "convenc", "debat", "gan",
            "meta", "proyecto", "decid", "compet", "dinero", "marketing", "coordin"
        ],
        [EAffinityDimension.Conventional] =
        [
            "orden", "planific", "archiv", "registr", "contab", "precis", "detall", "norma", "agenda", "lista",
            "calcul", "administr", "estructur", "puntual", "metod", "excel"
        ]
    };

    public static AffinityProfile Analyze(IEnumerable<TestQuestion> questions,
        IReadOnlyDictionary<Guid, string> answers)
    {
        var raw = Enum.GetValues<EAffinityDimension>().ToDictionary(dimension => dimension, _ => 0);

        foreach (var question in questions)
        {
            if (!answers.TryGetValue(question.Id.Value, out var answer) || !question.IsAnswered(answer)) continue;

            if (question.IsOpen)
            {
                if (question.Area is { } area) raw[area] += OpenQuestionAreaWeight;
                var text = Normalize(answer);
                foreach (var (dimension, stems) in Keywords)
                    raw[dimension] += stems.Count(stem => text.Contains(stem));
            }
            else
            {
                var option = question.Options.FirstOrDefault(o => o.Code == answer);
                if (option is not null) raw[option.Area] += ChoiceOptionWeight;
            }
        }

        var max = Math.Max(raw.Values.Max(), 1);

        int Scale(EAffinityDimension dimension) =>
            (int)Math.Round(MinScore + (double)raw[dimension] / max * (100 - MinScore),
                MidpointRounding.AwayFromZero);

        return new AffinityProfile(
            Scale(EAffinityDimension.Artistic),
            Scale(EAffinityDimension.Social),
            Scale(EAffinityDimension.Investigative),
            Scale(EAffinityDimension.Enterprising),
            Scale(EAffinityDimension.Conventional));
    }

    /// <summary>Lower case and removes accents: "Diseño" becomes "diseno".</summary>
    private static string Normalize(string text)
    {
        var decomposed = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
        return new string(decomposed
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray());
    }
}
