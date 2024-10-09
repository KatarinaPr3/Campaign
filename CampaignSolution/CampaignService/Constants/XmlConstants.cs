using CampaignService.Enums;
using CampaignService.Helpers;

namespace CampaignService.Constants
{
    public class XmlConstants
    {
        public const string XNAMESPACE_NS = "http://tempuri.org";
        public const string XSINAMESPACE = "http://www.w3.org/2001/XMLSchema-instance";
        public static readonly string[] NON_AGENT_JOB_TITLES = new[]
       {
            EnumHelpers.GetEnumDescription(JobTitles.LaboratorySupportEngineer),
            EnumHelpers.GetEnumDescription(JobTitles.LaboratoryTechnician),
            EnumHelpers.GetEnumDescription(JobTitles.SeniorDeveloper),
            EnumHelpers.GetEnumDescription(JobTitles.AssociateDeveloper),
            EnumHelpers.GetEnumDescription(JobTitles.AssistantSystemsEngineer),
            EnumHelpers.GetEnumDescription(JobTitles.ResearchAsst),
            EnumHelpers.GetEnumDescription(JobTitles.AssistantResearchAsst),
            EnumHelpers.GetEnumDescription(JobTitles.LaboratoryAdministrator),
            EnumHelpers.GetEnumDescription(JobTitles.StrategicHygienist),
            EnumHelpers.GetEnumDescription(JobTitles.LaboratoryWebMaster),
            EnumHelpers.GetEnumDescription(JobTitles.SeniorWebMaster),
            EnumHelpers.GetEnumDescription(JobTitles.AssociateWebMaster),
            EnumHelpers.GetEnumDescription(JobTitles.LaboratoryAccountant),
            EnumHelpers.GetEnumDescription(JobTitles.GlobalHygienist)
        };

    }
}
