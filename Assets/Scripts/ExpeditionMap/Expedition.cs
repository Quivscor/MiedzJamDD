using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ExpeditionMap
{
    public class Expedition
    {
        public Field destinationField = null;

        private int timeToFinishExpedition = 0;
        private int currentTimeToFinishExpedition = 0;

        public Expedition(Field field)
        {
            this.destinationField = field;

            timeToFinishExpedition = field.DistanceToRoot + 2;

            TimeController.Instance.OnNextDay += OnNextDay;
        }

        private void OnNextDay()
        {
            currentTimeToFinishExpedition++;

            if (currentTimeToFinishExpedition >= timeToFinishExpedition)
            {
                ExpeditionFinished();
            }
        }

        private void ExpeditionFinished()
        {
            TimeController.Instance.OnNextDay -= OnNextDay;
            ExpeditionManager.Instance.ExpeditionFinished(OnExpeditionFinished());
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

            raportData.gainedCopper = gainedCopper;

            raportData.discoveredFieldsText = discoveredFields + "";

            raportData.contextText = GenerateRaportContent(); //Change to static method for generating context or something

            raportData.additionalActivity = additionalActivityOccured;

            raportData.additionalActivityText = additionalActivityOutput;

            return raportData;
        }

        private int gainedCopper = 0;
        private int discoveredFields = 0;
        private bool additionalActivityOccured = false;
        private string additionalActivityOutput = "";

        public int CurrentTimeToFinishExpedition { get => currentTimeToFinishExpedition;  }
        public int LeftTimeToFinishExpedition { get => timeToFinishExpedition - currentTimeToFinishExpedition; }

        private void CalculateGainingCopper()
        {
            int maxGainedCopper = (int)Mathf.Min((int)CategoriesProgressController.Instance.sciences[(int)CategoriesProgressController.ScienceCategory.Transport].level * TeamStatsModifiers.LoadModifier, destinationField.CurrentCopper);

            destinationField.CurrentCopper = (int)Mathf.Max(destinationField.CurrentCopper - maxGainedCopper, 0);

            gainedCopper = maxGainedCopper;

            //ResourceController.Instance?.AddCopper(gainedCopper);
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

        public string GenerateRaportContent()
        {
            int lengthFluffOption = Random.Range(0, 5);
            string expeditionLength = "";
            string miesiecy = "miesiące";
            if (timeToFinishExpedition >= 5)
                miesiecy = "miesięcy";
            switch (lengthFluffOption)
            {
                case 0:
                    expeditionLength = "Ekspedycja potrwała " + timeToFinishExpedition + " " + miesiecy +".\n";
                    break;
                case 1:
                    expeditionLength = "Prace kopalniane trzymały górników w pracy przez " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;
                case 2:
                    expeditionLength = "Po " + timeToFinishExpedition + " miesiącach prace zostały zakończone.\n";
                    break;
                case 3:
                    expeditionLength = "Długość wyprawy wyniosła " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;
                case 4:
                    expeditionLength = "Drążenie tuneli i pozyskanie miedzi zajęło nam " + timeToFinishExpedition + " " + miesiecy + ".\n";
                    break;

            }

            string miningStory = "";
            lengthFluffOption = Random.Range(0, 5);
            switch (lengthFluffOption)
            {
                case 0:
                    miningStory = "Prace przebiegły normalnie. Nie odnotowano żadnych wypadków ani zniszczeń.\n";
                    break;
                case 1:
                    miningStory = "Zanotowano wykorzystanie materiałów wybuchowych podczas wydobycia.\n";
                    break;
                case 2:
                    miningStory = "Część drążonego tunelu składała się z bardzo twardego materiału, ale nasze maszyny dały sobie radę.\n";
                    break;
                case 3:
                    miningStory = "Wydobyte pokłady miedzi pokryły się z naszymi szacowaniami.\n";
                    break;
                case 4:
                    miningStory = "Mimo rekordowych upałów udało się sprawnie przeprowadzić ekspedycję.\n";
                    break;
            }

            return expeditionLength + miningStory;
        }
    }
}
