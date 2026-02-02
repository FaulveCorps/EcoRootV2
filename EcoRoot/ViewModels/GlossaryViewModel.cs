using System.Collections.Generic;
using EcoRoot.Resources;

namespace EcoRoot.ViewModels;

public sealed class GlossaryViewModel : BaseViewModel
{
    public GlossaryViewModel()
    {
        Title = Strings.GlossaryTitle;
    }

    public string IntroText => Strings.GlossaryIntro;

    public IReadOnlyList<string> Terms { get; } = new[]
    {
        Strings.GlossaryTermSoilContamination,
        Strings.GlossaryTermHeavyMetals,
        Strings.GlossaryTermPesticideResidue,
        Strings.GlossaryTermLeaching,
        Strings.GlossaryTermRemediation,
        Strings.GlossaryTermBioaccumulation
    };

    public bool HasTerms => Terms.Count > 0;

    public bool IsTermsEmpty => !HasTerms;
}
