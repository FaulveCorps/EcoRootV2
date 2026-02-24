using EcoWay.Models;

namespace EcoWay.Services;

public class EcoRootContentService
{
    public IReadOnlyList<HeroStat> HeroStats { get; } =
    [
        new("33", "%", "of global soil is degraded"),
        new("24", "B", "tonnes of fertile soil lost yearly"),
        new("95", "%", "of our food depends on soil")
    ];

    public IReadOnlyList<CauseItem> Causes { get; } =
    [
        new("deforestation", "🌲", "Deforestation", "Trees removed, soil exposed.",
        [
            "Rapid nutrient loss and topsoil erosion.",
            "Higher landslide risk and watershed instability.",
            "Habitat collapse for soil organisms."
        ]),
        new("mining", "⛏️", "Mining Runoff", "Acid and heavy metals seep out.",
        [
            "Acid drainage contaminates nearby land.",
            "Heavy metals accumulate in soil and crops.",
            "Groundwater quality degrades over time."
        ]),
        new("industrial", "🏭", "Industrial Waste", "Toxic factory chemicals.",
        [
            "Persistent toxic compounds enter food systems.",
            "Lead and metals build up in topsoil.",
            "Nearby communities face long-term exposure risks."
        ]),
        new("pesticides", "🧪", "Pesticides", "Chemicals kill soil life.",
        [
            "Beneficial microbes and worms decline.",
            "Chemical residues build up in agricultural soils.",
            "Biodiversity and fertility gradually weaken."
        ]),
        new("waste", "🗑️", "Improper Waste", "Garbage contaminates land.",
        [
            "Leachate introduces toxins into soil layers.",
            "Pathogens and disease vectors increase.",
            "Local groundwater becomes vulnerable."
        ]),
        new("plastic", "🛢️", "Plastic Waste", "Microplastics infiltrate soil.",
        [
            "Plastic fragments alter soil structure.",
            "Additives leach into surrounding ground.",
            "Microplastic contamination enters food chains."
        ])
    ];

    public IReadOnlyList<ConsequenceItem> Consequences { get; } =
    [
        new("soil", "🌍", "Polluted Soil", "The source point where contamination starts spreading.",
        [
            "33% of Earth’s soil is already degraded.",
            "Recovery is extremely slow without intervention.",
            "Contamination migrates into connected systems."
        ]),
        new("plants", "🌾", "Crops & Plants", "Roots absorb nutrients and pollutants together.",
        [
            "Heavy metals can accumulate in edible crops.",
            "Yields decline as soil biology weakens.",
            "Food quality and safety become less reliable."
        ]),
        new("water", "💧", "Water Sources", "Pollutants leach from soil into water systems.",
        [
            "Groundwater contamination can persist for years.",
            "Irrigation can spread contamination wider.",
            "Safe drinking water access becomes harder."
        ]),
        new("animals", "🦎", "Wildlife", "Soil contamination disrupts habitat and food webs.",
        [
            "Earthworm and insect populations collapse first.",
            "Toxins bioaccumulate up the food chain.",
            "Local biodiversity and ecosystem resilience drop."
        ]),
        new("humans", "👨‍👩‍👧‍👦", "Human Health", "Exposure occurs through food, water, dust, and direct contact.",
        [
            "Increased risk of respiratory and neurological issues.",
            "Children are especially vulnerable to heavy metals.",
            "Long-term exposure raises chronic disease burden."
        ]),
        new("economy", "📉", "Economy", "Soil degradation creates hidden costs across sectors.",
        [
            "Lower farm productivity and income loss.",
            "Costly remediation and public health expenses.",
            "Property and land value decline near hotspots."
        ]),
        new("climate", "🌡️", "Climate", "Damaged soils release carbon and worsen climate stress.",
        [
            "Healthy soil stores massive carbon reserves.",
            "Degradation releases stored greenhouse gases.",
            "Climate impacts accelerate further soil loss."
        ])
    ];

    public IReadOnlyList<SolutionItem> Solutions { get; } =
    [
        new("phytoremediation", "🌻", "Phytoremediation", "Plants absorb and stabilize contaminants.",
        [
            "Removes or locks pollutants in plant tissue.",
            "Improves biodiversity and root structure.",
            "Provides lower-cost long-term cleanup."
        ]),
        new("composting", "🍂", "Composting", "Organic waste becomes nutrient-rich soil input.",
        [
            "Improves moisture retention and fertility.",
            "Adds beneficial microorganisms.",
            "Reduces landfill methane emissions."
        ]),
        new("bioremediation", "🦠", "Bioremediation", "Microbes break down toxic compounds naturally.",
        [
            "Targets oils, pesticides, and organics.",
            "Reduces pollutant toxicity over time.",
            "Supports natural ecosystem recovery."
        ]),
        new("recycling", "♻️", "Recycling", "Keep hazardous and reusable materials out of soil.",
        [
            "Cuts landfill and open-dump pressure.",
            "Prevents toxic leakage into ground.",
            "Lowers raw-material extraction demand."
        ]),
        new("segregation", "🗂️", "Waste Segregation", "Separate waste streams at the source.",
        [
            "Stops toxic mixing and accidental leaks.",
            "Improves safe disposal compliance.",
            "Boosts recycling and treatment efficiency."
        ]),
        new("covercrops", "🌿", "Cover Crops", "Protect and enrich soil between seasons.",
        [
            "Reduces erosion and nutrient runoff.",
            "Adds organic matter and root carbon.",
            "Suppresses weeds and supports microbes."
        ])
    ];

    public IReadOnlyList<FaqItem> FaqItems { get; } =
    [
        new("🌍", "What is soil pollution and why should I care?", "Soil pollution is contamination of land by harmful substances. It matters because soil supports food, water filtration, biodiversity, and climate stability."),
        new("🏭", "What are the main causes of soil pollution?", "Major causes include industrial waste, mining runoff, pesticide overuse, plastic accumulation, improper disposal, and deforestation."),
        new("⚠️", "How does soil pollution affect human health?", "Through contaminated food, water, and dust exposure. Long-term exposure can increase respiratory, neurological, and chronic health risks."),
        new("🌱", "Can polluted soil be restored?", "Yes. Methods like phytoremediation, bioremediation, composting, cover crops, and safer waste systems can restore function over time."),
        new("🏡", "What can homeowners do immediately?", "Use organic alternatives, compost at home, dispose of chemicals correctly, and test soil quality if risk factors exist."),
        new("🚜", "How can farmers protect soil and maintain yield?", "Adopt precision inputs, crop rotation, cover crops, integrated pest management, and reduced tillage practices."),
        new("🔬", "How can I test soil for contamination?", "Start with basic kits, then use certified labs for heavy metals and chemical residues, especially near industrial or high-risk areas."),
        new("🌎", "How urgent is this crisis?", "Very urgent—degraded soil threatens food security, ecosystem health, and climate resilience. Early action prevents long-term irreversible damage."),
        new("💡", "How can students and communities help?", "Organize cleanups, school gardens, awareness campaigns, and policy advocacy to build long-term local stewardship."),
        new("📚", "Where can I learn more and stay involved?", "Follow credible environmental organizations, local extension services, and community programs focused on soil restoration."),
    ];

    private readonly Dictionary<string, ActionPlanDefinition> _actionPlans = new(StringComparer.OrdinalIgnoreCase)
    {
        ["homeowner:urban"] = new(
            "Urban Homeowner Action Plan",
            "Make cleaner soil choices in city living.",
            [
                new("Test your soil", "Use a local lab or test kit before planting.", 5),
                new("Start a compost setup", "Convert food scraps into organic fertilizer.", 8),
                new("Swap to organic yard inputs", "Replace harsh chemicals with safer options.", 6)
            ],
            [
                new("Install a rain garden", "Filter runoff before it reaches drains.", 15),
                new("Plant native species", "Reduce chemical dependency and support biodiversity.", 12),
                new("Join a neighborhood cleanup", "Reduce waste exposure hot spots.", 10)
            ],
            [
                new("Advocate local green-space policy", "Support long-term soil protection measures.", 20),
                new("Mentor nearby households", "Share practical soil-safe habits.", 15),
                new("Track neighborhood risk sources", "Stay informed and report hazardous dumping.", 18)
            ]),

        ["homeowner:suburban"] = new(
            "Suburban Homeowner Action Plan",
            "Protect your home ecosystem from preventable contamination.",
            [
                new("Audit lawn chemicals", "Identify products to phase out this week.", 7),
                new("Build compost bins", "Recycle green waste into nutrients.", 9),
                new("Mulch exposed beds", "Protect topsoil from erosion and heat stress.", 6)
            ],
            [
                new("Add permeable pathways", "Reduce polluted runoff.", 16),
                new("Create pollinator strips", "Boost biodiversity and soil biology.", 14),
                new("Set neighborhood recycling norms", "Improve source segregation consistency.", 11)
            ],
            [
                new("Convert high-input lawn sections", "Replace with native ground cover.", 22),
                new("Install rainwater reuse", "Lower stormwater and contamination pathways.", 18),
                new("Organize community action days", "Scale impact beyond one property.", 25)
            ]),

        ["homeowner:rural"] = new(
            "Rural Homeowner Action Plan",
            "Steward land health for long-term resilience.",
            [
                new("Run a full soil test", "Check pH, nutrients, and contamination markers.", 8),
                new("Map contamination risks", "Identify runoff and legacy dump points.", 7),
                new("Protect wells and streams", "Establish immediate buffer strips.", 10)
            ],
            [
                new("Plant seasonal cover crops", "Protect soil between active use periods.", 15),
                new("Scale compost operation", "Return nutrients to degraded patches.", 18),
                new("Install windbreak rows", "Reduce erosion loss and dust movement.", 14)
            ],
            [
                new("Adopt rotational land practices", "Allow stressed zones to recover.", 25),
                new("Designate restoration zones", "Rebuild biodiversity corridors.", 30),
                new("Join local conservation programs", "Secure long-term land stewardship support.", 28)
            ]),

        ["farmer:urban"] = new(
            "Urban Farmer Action Plan",
            "Grow safe food in dense environments.",
            [
                new("Test all beds", "Screen for lead and heavy metals.", 10),
                new("Use clean growing media", "Verify imported compost/soil quality.", 8),
                new("Use raised barriers", "Isolate crops from suspect ground.", 9)
            ],
            [
                new("Deploy phytoremediation strips", "Absorb contaminants in non-food zones.", 18),
                new("Educate customers", "Share food-safety and soil-health practices.", 12),
                new("Close organic waste loop", "Compost and reuse nutrients on-site.", 16)
            ],
            [
                new("Create urban grower network", "Scale shared best practices.", 22),
                new("Advocate mandatory site testing", "Improve local food safety policy.", 25),
                new("Run grower workshops", "Train new farmers on contamination prevention.", 20)
            ]),

        ["farmer:suburban"] = new(
            "Suburban Farmer Action Plan",
            "Build productive farms with regenerative safeguards.",
            [
                new("Baseline soil assessment", "Capture full-field health metrics.", 9),
                new("Reduce synthetic load", "Lower chemical input intensity.", 10),
                new("Filter irrigation inflow", "Protect fields from upstream pollution.", 8)
            ],
            [
                new("Adopt low-disturbance tillage", "Preserve soil structure and microbes.", 18),
                new("Plant pollinator hedges", "Stabilize ecosystems around plots.", 14),
                new("Create local CSA demand", "Support sustainable production cycles.", 15)
            ],
            [
                new("Pursue organic/regenerative certification", "Institutionalize best practices.", 28),
                new("Implement agroforestry blocks", "Increase resilience and carbon capture.", 30),
                new("Host open-farm demos", "Share proven techniques regionally.", 25)
            ]),

        ["farmer:rural"] = new(
            "Rural Farmer Action Plan",
            "Lead large-scale land regeneration.",
            [
                new("Map full-field variability", "Target interventions by soil condition.", 12),
                new("Cut high-risk chemical use", "Prioritize safer alternatives first.", 11),
                new("Protect riparian corridors", "Reduce nutrient and toxin transport.", 10)
            ],
            [
                new("Diversify cover crop rotations", "Improve structure and nutrient cycling.", 20),
                new("Integrate managed grazing", "Rebuild organic matter naturally.", 22),
                new("Join regional farmer circles", "Accelerate learning and adoption.", 15)
            ],
            [
                new("Complete regenerative transition", "Shift farm system to soil-first management.", 35),
                new("Participate in soil-carbon programs", "Monetize and scale restoration.", 30),
                new("Become a demonstration site", "Train peers using real outcomes.", 28)
            ]),

        ["student:urban"] = new(
            "Urban Student Action Plan",
            "Turn awareness into measurable local action.",
            [
                new("Research your district", "Identify known contamination concerns.", 5),
                new("Start a mini garden", "Learn soil health through practice.", 8),
                new("Reduce daily waste", "Prevent avoidable landfill leakage.", 6)
            ],
            [
                new("Organize cleanup days", "Remove waste before it degrades soil.", 12),
                new("Launch awareness content", "Educate peers with credible data.", 10),
                new("Propose school composting", "Create a practical circular system.", 14)
            ],
            [
                new("Lead an eco club", "Sustain long-term student initiatives.", 20),
                new("Partner with universities", "Support citizen-science monitoring.", 18),
                new("Advocate curriculum updates", "Embed soil stewardship in education.", 22)
            ]),

        ["student:suburban"] = new(
            "Suburban Student Action Plan",
            "Make your neighborhood greener and cleaner.",
            [
                new("Audit home products", "Swap risky garden chemicals.", 6),
                new("Start backyard restoration", "Plant natives and improve soil cover.", 7),
                new("Learn local ecology", "Understand your watershed and soil context.", 5)
            ],
            [
                new("Map local risk points", "Document dumping and erosion spots.", 11),
                new("Create youth volunteer team", "Coordinate recurring action days.", 13),
                new("Partner with local shops", "Sponsor bins and awareness drives.", 12)
            ],
            [
                new("Build a permanent youth program", "Create durable community ownership.", 22),
                new("Collaborate with researchers", "Contribute to local data collection.", 18),
                new("Run for student leadership", "Push school sustainability policy.", 20)
            ]),

        ["student:rural"] = new(
            "Rural Student Action Plan",
            "Protect agricultural heritage with modern practices.",
            [
                new("Document land history", "Track past use and contamination clues.", 6),
                new("Join agri clubs", "Learn best practices from mentors.", 8),
                new("Support home composting", "Improve nutrient cycling locally.", 7)
            ],
            [
                new("Start a soil monitoring project", "Measure trends over seasons.", 14),
                new("Create youth farm networks", "Share sustainable methods.", 12),
                new("Design a sustainability mini-plan", "Set practical yearly targets.", 15)
            ],
            [
                new("Pursue agri or soil science training", "Build advanced restoration skills.", 25),
                new("Support family succession planning", "Preserve sustainable land stewardship.", 28),
                new("Lead regional youth programs", "Scale adoption in nearby communities.", 24)
            ]),

        ["business:urban"] = new(
            "Urban Business Action Plan",
            "Reduce business footprint and lead visible change.",
            [
                new("Audit waste streams", "Identify high-risk disposal points.", 8),
                new("Switch to soil-safe products", "Replace harsh chemicals in operations.", 6),
                new("Assess property contamination", "Test vulnerable outdoor areas.", 9)
            ],
            [
                new("Implement zero-waste workflows", "Cut landfill-bound waste rapidly.", 18),
                new("Add green infrastructure", "Install planters/rain gardens for runoff capture.", 14),
                new("Source from sustainable suppliers", "Support cleaner upstream systems.", 16)
            ],
            [
                new("Earn environmental certification", "Institutionalize accountability.", 28),
                new("Form a local business coalition", "Coordinate district-wide impact.", 25),
                new("Fund restoration initiatives", "Invest directly in soil recovery.", 30)
            ]),

        ["business:suburban"] = new(
            "Suburban Business Action Plan",
            "Build sustainable operations with local influence.",
            [
                new("Review supply risks", "Identify contamination exposure points.", 9),
                new("Landscape organically", "Eliminate chemical-heavy maintenance.", 7),
                new("Secure hazardous disposal", "Prevent accidental leakage.", 10)
            ],
            [
                new("Install runoff capture", "Treat parking-lot and roof flow.", 16),
                new("Train employees on soil-safe practice", "Create shared operational standards.", 14),
                new("Support local eco groups", "Back practical restoration projects.", 12)
            ],
            [
                new("Retrofit facility systems", "Minimize ground contamination risk.", 28),
                new("Publish sector best practices", "Raise environmental baseline in your industry.", 32),
                new("Build supplier mentoring program", "Spread cleaner methods across the chain.", 25)
            ]),

        ["business:rural"] = new(
            "Rural Business Action Plan",
            "Grow responsibly while protecting working land.",
            [
                new("Run environmental baseline checks", "Measure current soil and water risk.", 10),
                new("Harden chemical storage", "Eliminate leak pathways.", 9),
                new("Protect nearby water bodies", "Deploy immediate safeguards.", 11)
            ],
            [
                new("Apply best management practices", "Standardize contamination prevention.", 18),
                new("Build vegetative buffers", "Reduce runoff and sediment movement.", 16),
                new("Partner with local producers", "Support regenerative local economies.", 14)
            ],
            [
                new("Become a regional model operator", "Demonstrate profitable sustainability.", 30),
                new("Co-fund conservation projects", "Scale restoration beyond property bounds.", 28),
                new("Document succession sustainability", "Ensure standards persist long-term.", 25)
            ]),
    };

    public IReadOnlyList<string> Roles { get; } = ["homeowner", "farmer", "student", "business"];
    public IReadOnlyList<string> Locations { get; } = ["urban", "suburban", "rural"];

    public string GetRoleLabel(string role) => role.ToLowerInvariant() switch
    {
        "homeowner" => "🏠 Homeowner",
        "farmer" => "🚜 Farmer",
        "student" => "📚 Student",
        "business" => "🏢 Business Owner",
        _ => "🌱 Community Member"
    };

    public string GetLocationLabel(string location) => location.ToLowerInvariant() switch
    {
        "urban" => "🏙️ Urban Area",
        "suburban" => "🏘️ Suburban Area",
        "rural" => "🌾 Rural Area",
        _ => "🌍 Any Area"
    };

    public ActionPlanDefinition? GetActionPlan(string role, string location)
    {
        return _actionPlans.TryGetValue($"{role}:{location}", out var plan)
            ? plan
            : null;
    }
}
