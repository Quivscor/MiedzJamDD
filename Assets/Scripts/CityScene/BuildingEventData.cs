using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEventData
{
    public string buildingName;
    public string category;
    public string description;
    public string bonuses;

    public BuildingEventData(string buildingName, CategoriesProgressController.ScienceCategory category, string description, string bonuses)
    {
        this.buildingName = GetCategoryColor(category) + buildingName + "</color>";
        this.category = GetCategoryColor(category) + category.ToString() + "</color>";
        this.description = description;
        this.bonuses = bonuses;
    }

    public static string GetCategoryColor(CategoriesProgressController.ScienceCategory category)
    {
        switch(category)
        {
            case CategoriesProgressController.ScienceCategory.Energetyka:
                return "<color=#FFFA34>";
            case CategoriesProgressController.ScienceCategory.Telekomunikacja:
                return "<color=#B900F8>";
            case CategoriesProgressController.ScienceCategory.Transport:
                return "<color=#CD1725>";
            case CategoriesProgressController.ScienceCategory.Społeczność:
                return "<color=#00B917>";
        }

        return "<color=black>";
    }

}
