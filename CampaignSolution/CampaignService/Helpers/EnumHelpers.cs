using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignService.Helpers
{
    public class EnumHelpers
    {
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
