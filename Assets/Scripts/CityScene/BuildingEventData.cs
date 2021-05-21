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
        this.buildingName = "<color=" + GetCategoryColor(category) + ">" + buildingName + "</color>";
        this.category = "<color=" + GetCategoryColor(category) + ">" + category.ToString() + "</color>";
        this.description = description;
        this.bonuses = bonuses;
    }

    private string GetCategoryColor(CategoriesProgressController.ScienceCategory category)
    {
        switch(category)
        {
            case CategoriesProgressController.ScienceCategory.Energetyka:
                return "#FFFA34";
            case CategoriesProgressController.ScienceCategory.Telekomunikacja:
                return "#B900F8";
            case CategoriesProgressController.ScienceCategory.Transport:
                return "#A40713";
            case CategoriesProgressController.ScienceCategory.Rolnictwo:
                return "#00B917";
        }

        return "black";
    }

}
