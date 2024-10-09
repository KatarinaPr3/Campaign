using System.ComponentModel;

namespace CampaignService.Enums
{
    public enum EnumsResponses
    {
        PersonIdentification,
        FindPersonResponse,
        FindPersonResult,
        ID,
        Name,
        SSN,
        DOB,
        Home,
        Street,
        City,
        State,
        Zip,
        Office,
        Spouse,
        FavoriteColors,
        FavoriteColorsItem,
        Age,
        Title,
        Salary,
        type
    }
    public enum JobTitles
    {
        [Description("Executive Technician")]
        ExecutiveTechnician,

        [Description("Associate Product Manager")]
        AssociateProductManager,

        [Description("Accountant")]
        Accountant,

        [Description("Associate Developer")]
        AssociateDeveloper,

        [Description("Executive Director")]
        ExecutiveDirector,

        [Description("Laboratory Support Engineer")]
        LaboratorySupportEngineer,

        [Description("Associate Accountant")]
        AssociateAccountant,

        [Description("Strategic Product Specialist")]
        StrategicProductSpecialist,

        [Description("Associate Marketing Manager")]
        AssociateMarketingManager,

        [Description("Laboratory Administrator")]
        LaboratoryAdministrator,

        [Description("Assistant Product Specialist")]
        AssistantProductSpecialist,

        [Description("Senior WebMaster")]
        SeniorWebMaster,

        [Description("Senior Product Specialist")]
        SeniorProductSpecialist,

        [Description("Strategic Resources Director")]
        StrategicResourcesDirector,

        [Description("Strategic Technician")]
        StrategicTechnician,

        [Description("Assistant Research Asst.")]
        AssistantResearchAsst,

        [Description("Global Accounts Rep.")]
        GlobalAccountsRep,

        [Description("Assistant Director")]
        AssistantDirector,

        [Description("Laboratory Sales Rep.")]
        LaboratorySalesRep,

        [Description("Assistant Systems Engineer")]
        AssistantSystemsEngineer,

        [Description("Executive WebMaster")]
        ExecutiveWebMaster,

        [Description("Systems Engineer")]
        SystemsEngineer,

        [Description("Associate Product Specialist")]
        AssociateProductSpecialist,

        [Description("Laboratory Technician")]
        LaboratoryTechnician,

        [Description("Associate Technician")]
        AssociateTechnician,

        [Description("Assistant Engineer")]
        AssistantEngineer,

        [Description("Senior Developer")]
        SeniorDeveloper,

        [Description("Senior Research Asst.")]
        SeniorResearchAsst,

        [Description("Global Accountant")]
        GlobalAccountant,

        [Description("Global Hygienist")]
        GlobalHygienist,

        [Description("Technician")]
        Technician,

        [Description("Associate Accounts Rep.")]
        AssociateAccountsRep,

        [Description("Assistant Technician")]
        AssistantTechnician,

        [Description("Associate Sales Rep.")]
        AssociateSalesRep,

        [Description("Strategic Accounts Rep.")]
        StrategicAccountsRep,

        [Description("Associate Engineer")]
        AssociateEngineer,

        [Description("Strategic Systems Engineer")]
        StrategicSystemsEngineer,

        [Description("Global Hygienist")]
        GlobalHygienist2,  // Avoid duplicate enum names

        [Description("Assistant Developer")]
        AssistantDeveloper,

        [Description("Global Product Manager")]
        GlobalProductManager,

        [Description("Strategic Hygienist")]
        StrategicHygienist,

        [Description("Engineer")]
        Engineer,

        [Description("Research Asst.")]
        ResearchAsst,

        [Description("Laboratory WebMaster")]
        LaboratoryWebMaster,

        [Description("Senior Accountant")]
        SeniorAccountant,

        [Description("Global Systems Engineer")]
        GlobalSystemsEngineer,

        [Description("Executive Sales Rep.")]
        ExecutiveSalesRep,

        [Description("Laboratory Accountant")]
        LaboratoryAccountant,

        [Description("Assistant Accountant")]
        AssistantAccountant,

        [Description("Executive Support Engineer")]
        ExecutiveSupportEngineer,

        [Description("Associate Director")]
        AssociateDirector,

        [Description("Senior Administrator")]
        SeniorAdministrator,

        [Description("Strategic Technician")]
        StrategicTechnician2,  // Avoid duplicate enum names

        [Description("Senior Technician")]
        SeniorTechnician,

        [Description("Global Engineer")]
        GlobalEngineer,

        [Description("Strategic Administrator")]
        StrategicAdministrator,

        [Description("Senior Marketing Manager")]
        SeniorMarketingManager,

        [Description("Laboratory Resources Director")]
        LaboratoryResourcesDirector,

        [Description("Laboratory Director")]
        LaboratoryDirector,

        [Description("Support Engineer")]
        SupportEngineer,

        [Description("Assistant WebMaster")]
        AssistantWebMaster,

        [Description("Senior Support Engineer")]
        SeniorSupportEngineer,

        [Description("Associate WebMaster")]
        AssociateWebMaster,

        [Description("Global Support Engineer")]
        GlobalSupportEngineer,

        [Description("Strategic Director")]
        StrategicDirector,

        [Description("Strategic Accountant")]
        StrategicAccountant,

        [Description("Laboratory Marketing Manager")]
        LaboratoryMarketingManager,

        [Description("Laboratory Developer")]
        LaboratoryDeveloper
    }

    public enum CampaignType
    {
        Discount = 0,
        Free = 1,
        BonusForNextPurchase = 2
    }

}
