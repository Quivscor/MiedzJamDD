using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ExpeditionMap
{
    public class Expedition
    {
        public Field destinationField = null;

        public Expedition(Field field)
        {
            this.destinationField = field;
        }

        public RaportData OnExpeditionFinished()
        {
            destinationField?.SetAsExpeditionTarget(false);

            CalculateGainingCopper();

            CalculateDiscoveringNeighbourFields();

            CalculateChancesForAdditionalActions();

            return GenerateRaport();
        }

        private RaportData GenerateRaport()
        {
            RaportData raportData = new RaportData();

            raportData.fieldCoordinatesText = "[ " + destinationField.FieldCoords.x + ", " + destinationField.FieldCoords.y + " ]";

            raportData.gainedCopperText = gainedCopper + "";

            raportData.discoveredFieldsText = discoveredFields + "";

            raportData.contextText = "No i co pajacu?"; //Change to static method for generating context or something

            raportData.additionalActivity = additionalActivityOccured;

            raportData.additionalActivityText = additionalActivityOutput;

            return raportData;
        }

        private int gainedCopper = 0;
        private int discoveredFields = 0;
        private bool additionalActivityOccured = false;
        private string additionalActivityOutput = "";

        private void CalculateGainingCopper()
        {
            int maxGainedCopper = (int)Mathf.Min(TeamStats.Instance.MaximumLoad * TeamStatsModifiers.LoadModifier, destinationField.CurrentCopper);

            destinationField.CurrentCopper = (int)Mathf.Max(destinationField.CurrentCopper - maxGainedCopper, 0);

            gainedCopper = maxGainedCopper;

            ResourceController.Instance?.AddCopper(gainedCopper);
        }

        private void CalculateDiscoveringNeighbourFields()
        {
            Field[] neighbours = ExpeditionMapManager.Instance.GetAllNeighbours(destinationField);

            System.Random random = new System.Random();

            neighbours = neighbours.OrderBy(x => random.Next()).ToArray();

            int fieldsToDiscover = 1;

            if (Random.Range(0, 100) <= TeamStats.Instance.ChanceToDiscoverAdditionalNeighbourField)
                fieldsToDiscover++;

            discoveredFields = 0;

            Debug.Log(fieldsToDiscover);

            for (int i = 0; i < neighbours.Length; i++)
            {
                if (!neighbours[i].HasBeenDiscovered)
                {
                    neighbours[i].HasBeenDiscovered = true;
                    fieldsToDiscover--;
                    discoveredFields++;

                    if (fieldsToDiscover <= 0)
                        break;
                }
            }
        }

        private void CalculateChancesForAdditionalActions()
        {
            //Implement later
            //Add smth to managers or increase smth
            additionalActivityOccured = false;
            additionalActivityOutput = "Nothing happened";
        }
    }
}
